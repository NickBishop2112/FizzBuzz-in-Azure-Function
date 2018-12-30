namespace FizzBuzz.Application
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using System;

    public static class FizzBuzzGenerator
    {
        [FunctionName("Generate")]
        [return: Queue("fizzbuzz-messages-002")]
        public static string Generate(
            [QueueTrigger("fizzbuzz-messages-001")]string queueItem,
            ILogger log)
        {
            try
            {
                int number;
                if (!int.TryParse(queueItem, out number))
                {
                    throw new InvalidOperationException($"Fizz Buzz input '{queueItem}' should be a integer");
                }

                string result = string.Empty;

                if ((number % 3) == 0)
                {
                    result += "Fizz";
                }

                if ((number % 5) == 0)
                {
                    result += "Buzz";
                }

                log.LogInformation($"Item '{number}' is processed");
                return result;
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                throw;
            }
        }
    }
}
