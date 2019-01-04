namespace FizzBuzz.Client.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Moq;
    using System;
    using System.Threading.Tasks;

    [TestClass]

    public class QueueTests
    {
        private Mock<CloudQueueClient> cloudQueueClient;

        private Mock<CloudQueue> cloudQueue;

        private Queue queue;

        [TestInitialize]
        public void TestInitialise()
        {
            this.cloudQueueClient = new Mock<CloudQueueClient>(MockBehavior.Loose, new Uri("http://test"), new StorageCredentials());
            this.cloudQueue = new Mock<CloudQueue>(MockBehavior.Loose, new Uri("http://test"));
            this.queue = new Queue(this.cloudQueueClient.Object, "inputQueueName", "outputQueueName");
        }

        [TestMethod]
        public async Task GivenWriteAsyncWhenMessageSuppliedThenQueued()
        {
            // Arrange
            this.cloudQueueClient.Setup(x => x.GetQueueReference("inputQueueName"))
                .Returns(this.cloudQueue.Object);

            // Act
            await queue.WriteAsync("3");

            // Assert
            this.cloudQueue.Verify(x => x.AddMessageAsync(It.IsAny<CloudQueueMessage>()), Times.Once);
        }

        [TestMethod]
        public async Task GivenReadAsyncWhenMessageFoundThenDequeued()
        {
            // Arrange
            this.cloudQueueClient.Setup(x => x.GetQueueReference("outputQueueName"))
                .Returns(this.cloudQueue.Object);

            this.cloudQueue.Setup(x => x.GetMessageAsync())
                .ReturnsAsync(new CloudQueueMessage("Fizz"));

            // Act
            var result = await new Queue(this.cloudQueueClient.Object, "inputQueueName", "outputQueueName").ReadAsync();

            // Assert
            ////result.Should().Be("Fizz");            
        }
    }
}
