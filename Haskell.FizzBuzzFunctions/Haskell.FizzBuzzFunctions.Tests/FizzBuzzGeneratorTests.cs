namespace FizzBuzz.Application.Tests
{
    using FizzBuzz.Application;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Internal;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class FizzBuzzGeneratorTests
    {
        private Mock<ILogger> logger;

        [TestInitialize]
        public void TestInitialise()
        {
            this.logger = new Mock<ILogger>();
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfThreeThenFizz()
        {
            var result = FizzBuzzGenerator.Generate("3", this.logger.Object);

            result.Should().Be(new KeyValuePair<string, string>("3", "Fizz"));
            this.VerifyLogging("Queue Item '3', Item '3' with result of 'Fizz' has been processed.", LogLevel.Information);
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfFiveThenBuzz()
        {
            var result = FizzBuzzGenerator.Generate("5", this.logger.Object);
            result.Should().Be(new KeyValuePair<string, string>("5", "Buzz"));
            this.VerifyLogging("Queue Item '5', Item '5' with result of 'Buzz' has been processed.", LogLevel.Information);
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfThreeAndFiveThenFizzBuzz()
        {
            var result = FizzBuzzGenerator.Generate("15", this.logger.Object);
            result.Should().Be(new KeyValuePair<string, string>("15", "FizzBuzz"));
            this.VerifyLogging("Queue Item '15', Item '15' with result of 'FizzBuzz' has been processed.", LogLevel.Information);
        }

        [TestMethod]
        public void GivenGenerateWhenNeitherMultiplesOfThreeOrFiveThenNoFizzAndOrBuzz()
        {
            var result = FizzBuzzGenerator.Generate("1", this.logger.Object);
            result.Should().Be(new KeyValuePair<string, string>("1", string.Empty));
            this.VerifyLogging("Queue Item '1', Item '1' with result of '' has been processed.", LogLevel.Information);
        }
      
        [TestMethod]
        public void GivenGenerateWhenInvalidNumberThenHandleException()
        {
            Action action = () => FizzBuzzGenerator.Generate("x", this.logger.Object);
            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Fizz Buzz input 'x' should be a integer"); 
            this.VerifyLogging("System.InvalidOperationException: Fizz Buzz input 'x' should be a integer", LogLevel.Error);            
        }

        private void VerifyLogging(string message, LogLevel logLevel)
        {
            this.logger
                .Verify(
                    x => 
                        x.Log(
                            logLevel, 
                            It.IsAny<EventId>(), 
                            It.Is<FormattedLogValues>(y => y[0].Value.ToString().StartsWith(message, StringComparison.Ordinal)), 
                            It.IsAny<Exception>(), 
                            It.IsAny<Func<object, Exception, string>>()), 
                    Times.Once);
        }
    }
}
