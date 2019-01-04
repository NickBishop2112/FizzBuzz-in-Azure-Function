namespace FizzBuzz.Client
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    public class Queue : IQueue
    {
        private readonly CloudQueueClient cloudQueueClient;

        private readonly string inputQueueName;

        private readonly string outputQueueName;

        public Queue(CloudQueueClient cloudQueueClient, string inputQueueName, string outputQueueName)
        {
            this.inputQueueName = inputQueueName;
            this.outputQueueName = outputQueueName;
            this.cloudQueueClient = cloudQueueClient;
        }

        public async Task<IDictionary<string, string>> ReadAsync()
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.outputQueueName);
            IEnumerable<CloudQueueMessage> messages = await queue.GetMessagesAsync(5);

            return messages
                .Select(message => JsonConvert.DeserializeObject<KeyValuePair<string, string>>(message.AsString))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);            
        }

        public async Task WriteAsync(string content)
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.inputQueueName);
            await queue.AddMessageAsync(new CloudQueueMessage(content));
        }
    }
}
