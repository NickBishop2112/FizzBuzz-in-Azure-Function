namespace Haskell.FizzBuzzFunctions
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([QueueTrigger("FizzBuzz-messages-001", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
