namespace FizzBuzz.Client
{
    using System;
    using System.IO;

    public class FizzBuzzClient : IFizzBuzzClient
    {
        private TextWriter textWriter;
        private readonly IQueue queue;

        public FizzBuzzClient(TextWriter textWriter, IQueue queue)
        {
            this.textWriter = textWriter;
            this.queue = queue;
        }

        public void Show()
        {
            this.queue.WriteAsync("3");
            var result = this.queue.ReadAsync();
            this.textWriter.WriteLineAsync($"Number '3' is {result.Result}");
        }
    }
}