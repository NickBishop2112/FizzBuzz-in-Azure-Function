namespace FizzBuzz.Client.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class FizzBuzzClientTests
    {
        [TestMethod]
        public async Task GivenShowWhenNumberRangeSuppliedThenFizzOrBuzzAsync()
        {
            // Arrange
            var textWriter = new Mock<TextWriter>();
            var queue = new Mock<IQueueHandler>();
            var configuration = new Mock<IConfiguration>(MockBehavior.Loose);

            var numbers = new List<Tuple<int, string>>
            {
                Tuple.Create(1, "Requested Number is '1' and is ''"),
                Tuple.Create(2, "Requested Number is '2' and is ''"),
                Tuple.Create(3, "Requested Number is '3' and is 'Fizz'")
            };

            foreach (var number in numbers)
            {
                queue.Setup(x => x.WriteAsync(number.Item1.ToString()))
                    .Returns(Task.CompletedTask);
            }

            queue.SetupSequence(x => x.ReadAsync())
                .ReturnsAsync(new Dictionary<string, string>
                {
                    ["1"] = string.Empty,
                    ["2"] = string.Empty,
                    ["3"] = "Fizz"
                });
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(x => x.Value).Returns("10");
            configuration.Setup(x => x.GetSection("Delay")).Returns(configurationSection.Object);

            // Act
            var client = new FizzBuzzClient(textWriter.Object, queue.Object, configuration.Object);

            await client.ShowAsync(1, 3);

            // Assert
            queue.VerifyAll();
            queue.Verify(x => x.ClearAsync(), Times.Once);
            numbers.ForEach(x => textWriter.Verify(y => y.WriteLineAsync(x.Item2), Times.Once()));
       }
    }
}
