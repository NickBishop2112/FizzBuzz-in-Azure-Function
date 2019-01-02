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

        public void Show(int minimum, int maximum)
        {
            for (int index = minimum; index < (maximum + 1);  index++)
            {
                this.queue.WriteAsync(index.ToString());
                var result = this.queue.ReadAsync();
                this.textWriter.WriteLineAsync($"Requested Number is '{index}' and is '{result.Result}'");
            }            
        }
    }
}