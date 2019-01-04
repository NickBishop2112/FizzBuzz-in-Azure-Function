namespace FizzBuzz.Client.Tests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;

    [TestClass]
    public class FizzBuzzClientIntegrationTests
    {
        [TestMethod]
        public void GivenMainWhenCalledThenFizzBuzz()
        {
            // Arrange
            var stringBuilder = new StringWriter();
            Console.SetOut(stringBuilder);

            // Act
            Program.Main(new string[] { "--min=1", "--max=3" });

            // Assert
            stringBuilder.ToString().Should().Be("Sent Number is '1'\r\nRequested Number is '1' and is ''\r\n");
        }
    }
}
