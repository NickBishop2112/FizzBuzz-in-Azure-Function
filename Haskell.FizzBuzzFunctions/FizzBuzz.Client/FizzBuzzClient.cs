namespace FizzBuzz.Client
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class FizzBuzzClient : IFizzBuzzClient
    {
        private TextWriter textWriter;
        private readonly IQueueHandler queueHandler;
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public FizzBuzzClient(
            TextWriter textWriter, 
            IQueueHandler queueHandler, 
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.textWriter = textWriter;
            this.queueHandler = queueHandler;
            this.configuration = configuration;
        }

        public void ShowAsync(int minimum, int maximum)
        {
            Task.Run(() => this.queueHandler.ClearAsync());

            var slots = new Dictionary<string, bool>();

            async Task WriteAsync()
            {
                for (int index = minimum; index < (maximum + 1); index++)
                {
                    await this.textWriter.WriteLineAsync($"Sent Number is '{index}'").ConfigureAwait(false);
                    await this.queueHandler.WriteAsync(index.ToString());

                    slots.TryAdd(index.ToString(), false);
                }
            }

            async Task ReadAsync()
            {
                while (!slots.Any() || slots.Values.Any(isProcessed => !isProcessed))
                {
                    var result = this.queueHandler.ReadAsync();
                    foreach (var response in result.Result)
                    {
                        await this.textWriter.WriteLineAsync($"Requested Number is '{response.Key}' and is '{response.Value}'");
                        slots[response.Key] = true;
                    }
                }

                await Task.Delay(this.configuration.GetValue<int>("Delay")).ConfigureAwait(false);
            }
       
            Task.WaitAll(WriteAsync(),ReadAsync());
        }
    }
}