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
            string result;
            FizzBuzzGenerator.Generate("3", out result, this.logger.Object);

            result.Should().Be("Fizz");
            this.VerifyLogging("Item '3' is processed");
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfFiveThenBuzz()
        {
            string result;
            FizzBuzzGenerator.Generate("5", out result, this.logger.Object);
            result.Should().Be("Buzz");
            this.VerifyLogging("Item '5' is processed");
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfThreeAndFiveThenFizzBuzz()
        {
            string result;
            FizzBuzzGenerator.Generate("15", out result, this.logger.Object);
            result.Should().Be("FizzBuzz");
            this.VerifyLogging("Item '15' is processed");
        }

        [TestMethod]
        public void GivenGenerateWhenNeitherMultiplesOfThreeOrFiveThenNoFizzAndOrBuzz()
        {
            string result;
            FizzBuzzGenerator.Generate("1", out result, this.logger.Object);
            result.Should().Be(string.Empty);
            this.VerifyLogging("Item '1' is processed");
        }

        private void VerifyLogging(string message)
        {
            this.logger
                .Verify(
                    x => 
                        x.Log(
                            LogLevel.Information, 
                            It.IsAny<EventId>(), 
                            It.Is<FormattedLogValues>(y => y[0].Value.ToString() == message), 
                            null, 
                            It.IsAny<Func<object, Exception, string>>()), 
                    Times.Once);
        }

    }
}
