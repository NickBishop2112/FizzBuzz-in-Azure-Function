namespace FizzBuzz.Client.Tests
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FizzBuzzClientIntegrationTests
    {
        [TestMethod]
        public void GivenRunWhenCalledThenFizzBuzzForRangeOfNumbers()
        {
            // Arrange
            var stringBuilder = new StringWriter();
            Console.SetOut(stringBuilder);

            // Act
            Action action = () => Program.Main(new string[] { "--min=1", "--max=3" });

            // Assert
            action.Should().NotThrow<Exception>();
            stringBuilder.ToString().Should().NotBeEmpty();
        }

        [TestMethod]
        public void GivenRunWhenInvalidErrorThenLogError()
        {
            // Arrange
            var stringBuilder = new StringWriter();
            Console.SetError(stringBuilder);

            // Act
            Action action = () => Program.Main(new string[] { "--min=aaa", "--max=15" });

            // Assert
            action.Should().NotThrow<Exception>();
            stringBuilder.ToString().Should().NotBeEmpty();
        }
    }
}
