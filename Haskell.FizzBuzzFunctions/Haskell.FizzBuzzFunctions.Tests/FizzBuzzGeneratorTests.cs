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
            var result = FizzBuzzGenerator.Generate(3, this.logger.Object);
            
           result.Should().Be("Fizz");
           this.logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<FormattedLogValues>(y => y[0].Value.ToString() == "Item '3' is processed"), null, It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }
       
        [TestMethod]
        public void GivenGenerateWhenMultiplesOfFiveThenBuzz()
        {
            var result = FizzBuzzGenerator.Generate(5, this.logger.Object);
            result.Should().Be("Buzz");
        }

        [TestMethod]
        public void GivenGenerateWhenMultiplesOfThreeAndFiveThenFizzBuzz()
        {
            var result = FizzBuzzGenerator.Generate(15, this.logger.Object);
            result.Should().Be("FizzBuzz");
        }
    }
}
