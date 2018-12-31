namespace FizzBuzz.Client.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.IO;

    [TestClass]
    public class FizzBuzzClientTests
    {
        [TestMethod]
        public void GivenShowWhenNumberRangeSuppliedThenFizzOrBuzz()
        {
            // Arrange
            var textWriter = new Mock<TextWriter>();
            var queue = new Mock<IQueue>();
            queue.Setup(x => x.WriteAsync("3"));
            queue.Setup(x => x.ReadAsync()).ReturnsAsync("Fizz");
            
            // Act
            new FizzBuzz.Client.FizzBuzzClient(textWriter.Object, queue.Object).Show();

            // Assert
            queue.VerifyAll();
            textWriter.Verify(x => x.WriteLineAsync("Number '3' is Fizz"), Times.Once());
       }
    }
}
