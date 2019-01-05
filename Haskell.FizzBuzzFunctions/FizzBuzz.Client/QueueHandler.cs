namespace FizzBuzz.Client
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class QueueHandler : IQueueHandler
    {
        private readonly CloudQueueClient cloudQueueClient;

        private readonly string inputQueueName;

        private readonly string outputQueueName;

        public QueueHandler(CloudQueueClient cloudQueueClient, string inputQueueName, string outputQueueName)
        {
            this.inputQueueName = inputQueueName;
            this.outputQueueName = outputQueueName;
            this.cloudQueueClient = cloudQueueClient;
        }

        public async Task<IDictionary<string, string>> ReadAsync()
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.outputQueueName);
            IEnumerable<CloudQueueMessage> messages = await queue.GetMessagesAsync(5).ConfigureAwait(false);

            return messages
                .Select(message => JsonConvert.DeserializeObject<KeyValuePair<string, string>>(message.AsString))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);            
        }

        public async Task WriteAsync(string content)
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.inputQueueName);
            await queue.AddMessageAsync(new CloudQueueMessage(content)).ConfigureAwait(false);
        }

        public async Task ClearAsync()
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.outputQueueName);
            await queue.ClearAsync().ConfigureAwait(false);
        }
    }
}
