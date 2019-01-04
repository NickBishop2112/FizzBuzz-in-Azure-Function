namespace FizzBuzz.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
            var slots = new Dictionary<string, bool>();

            for (int index = minimum; index < (maximum + 1); index++)
            {
                this.textWriter.WriteLineAsync($"Sent Number is '{index}'");
                this.queue.WriteAsync(index.ToString());
                slots.Add(index.ToString(), false);

            }

            while (slots.Values.Any(isProcessed => !isProcessed))
            {
                var result = this.queue.ReadAsync();
                foreach (var response in result.Result)
                {
                    this.textWriter.WriteLineAsync($"Requested Number is '{response.Key}' and is '{response.Value}'");
                    slots[response.Key] = true;
                }            
            }
        }
    }
}