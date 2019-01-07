namespace FizzBuzz.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]

    public class QueueHandlerTests
    {
        private Mock<CloudQueueClient> cloudQueueClient;

        private Mock<CloudQueue> cloudQueue;

        private IQueueHandler queueHandler;

        [TestInitialize]
        public void TestInitialise()
        {
            this.cloudQueueClient = new Mock<CloudQueueClient>(MockBehavior.Loose, new Uri("http://test"), new StorageCredentials());
            this.cloudQueue = new Mock<CloudQueue>(MockBehavior.Loose, new Uri("http://test"));
            this.queueHandler = new QueueHandler(this.cloudQueueClient.Object, "inputQueueName", "outputQueueName");
        }

        [TestMethod]
        public async Task GivenWriteAsyncWhenMessageSuppliedThenQueued()
        {
            // Arrange
            this.cloudQueueClient.Setup(x => x.GetQueueReference("inputQueueName"))
                .Returns(this.cloudQueue.Object);

            // Act
            await this.queueHandler.WriteAsync("3");

            // Assert
            this.cloudQueue.Verify(x => x.AddMessageAsync(It.IsAny<CloudQueueMessage>()), Times.Once);
        }

        [TestMethod]
        public async Task GivenReadAsyncWhenMessagesFoundThenDequeued()
        {
            // Arrange
            this.cloudQueueClient.Setup(x => x.GetQueueReference("outputQueueName"))
                .Returns(this.cloudQueue.Object);

            var jsonObject = JsonConvert.SerializeObject(new KeyValuePair<string, string>("50", "Buzz"));

            this.cloudQueue.Setup(x => x.GetMessagesAsync(5))
                .ReturnsAsync(new List<CloudQueueMessage> { new CloudQueueMessage(jsonObject) });

            // Act
            var result = await this.queueHandler.ReadAsync();

            // Assert
            result.Should().BeEquivalentTo(new Dictionary<string, string> { ["50"] = "Buzz" });
        }

        [TestMethod]
        public async Task GivenReadAsyncWhenNoMessageFoundThenDequeued()
        {
            // Arrange
            this.cloudQueueClient.Setup(x => x.GetQueueReference("outputQueueName"))
                .Returns(this.cloudQueue.Object);

            this.cloudQueue.Setup(x => x.GetMessagesAsync(5))
                .ReturnsAsync(new List<CloudQueueMessage>());

            // Act
            var result = await this.queueHandler.ReadAsync();

            // Assert
            result.Should().BeEquivalentTo(new Dictionary<string, string>());
        }

        [TestMethod]
        public async Task GivenClearAsyncWhenMessageFoundThenQueueCleared()
        {
            // Arrange
            this.cloudQueueClient.Setup(x => x.GetQueueReference("outputQueueName"))
                .Returns(this.cloudQueue.Object);

            // Act
            await this.queueHandler.ClearAsync();

            // Assert
            this.cloudQueue.Verify(x => x.ClearAsync(), Times.Once);
        }
    }
}
