namespace FizzBuzz.Client
{
    using Microsoft.WindowsAzure.Storage.Queue;
    using System.Linq;
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

        public async Task<string> ReadAsync()
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.outputQueueName);
            return (await queue.GetMessageAsync()).AsString;
        }

        public async Task WriteAsync(string content)
        {
            var queue = this.cloudQueueClient.GetQueueReference(this.inputQueueName);
            await queue.AddMessageAsync(new CloudQueueMessage(content));
        }
    }
}
