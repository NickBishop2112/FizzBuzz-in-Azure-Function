namespace FizzBuzz.Application.Tests
{
    using FizzBuzz.Application;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Internal;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

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
            string result = FizzBuzzGenerator.Generate("3", this.logger.Object);

            result.Should().Be("Fizz");
            this.VerifyLogging("Item '3' is processed", LogLevel.Information);
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfFiveThenBuzz()
        {
            string result = FizzBuzzGenerator.Generate("5", this.logger.Object);
            result.Should().Be("Buzz");
            this.VerifyLogging("Item '5' is processed", LogLevel.Information);
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfThreeAndFiveThenFizzBuzz()
        {
            string result = FizzBuzzGenerator.Generate("15", this.logger.Object);
            result.Should().Be("FizzBuzz");
            this.VerifyLogging("Item '15' is processed", LogLevel.Information);
        }

        [TestMethod]
        public void GivenGenerateWhenNeitherMultiplesOfThreeOrFiveThenNoFizzAndOrBuzz()
        {
            string result = FizzBuzzGenerator.Generate("1", this.logger.Object);
            result.Should().Be(string.Empty);
            this.VerifyLogging("Item '1' is processed", LogLevel.Information);
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
