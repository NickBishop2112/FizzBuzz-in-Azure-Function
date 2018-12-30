namespace FizzBuzz.Application
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class FizzBuzzGenerator
    {
        [FunctionName("Generate")]
        public static void Generate(
            [QueueTrigger("fizzbuzz-messages-001")]string queueItem,
            [Queue("fizzbuzz-messages-002")] out string result,
            ILogger log)
        {
            int number = int.Parse(queueItem);
            result = string.Empty;

            if ((number % 3) == 0)
            {
                result += "Fizz";
            }

            if ((number % 5) == 0)
            {
                result += "Buzz";
            }

            log.LogInformation($"Item '{number}' is processed");            
        }
    }
}
