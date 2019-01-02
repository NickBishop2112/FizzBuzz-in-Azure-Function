namespace FizzBuzz.Client.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
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

            var numbers = new List<Tuple<int, string>>
            {
                Tuple.Create(1, "Requested Number is '1' and is ''"),
                Tuple.Create(2, "Requested Number is '2' and is ''"),
                Tuple.Create(3, "Requested Number is '3' and is 'Fizz'")
            };

            foreach (var number in numbers)
            {
                queue.Setup(x => x.WriteAsync(number.Item1.ToString()));
            }

            queue.SetupSequence(x => x.ReadAsync())
                .ReturnsAsync(string.Empty)
                .ReturnsAsync(string.Empty)
                .ReturnsAsync("Fizz");

            // Act
            new FizzBuzzClient(textWriter.Object, queue.Object).Show(1,3);

            // Assert
            queue.VerifyAll();

            numbers.ForEach(x => textWriter.Verify(y => y.WriteLineAsync(x.Item2), Times.Once()));
            
       }
    }
}
