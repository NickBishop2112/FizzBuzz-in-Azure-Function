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
            var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            Program.Main(new string[] { "--min=2", "--max=2" });

            // Assert
            sw.ToString().Should().Be("Sent Number is '1'\r\nRequested Number is '1' and is ''\r\n");
        }
    }
}
