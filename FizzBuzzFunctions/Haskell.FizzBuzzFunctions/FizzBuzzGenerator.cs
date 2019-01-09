namespace FizzBuzz.Application
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class FizzBuzzGenerator
    {
        [FunctionName("Generate")]
        [return: Queue("fizzbuzz-messages-002")]
        public static KeyValuePair<string, string> Generate(
            [QueueTrigger("fizzbuzz-messages-001")]string queueItem,
            ILogger log)
        {
            try
            {
                if (!int.TryParse(queueItem, out int number))
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

                log.LogInformation($"Queue Item '{queueItem}', Item '{number}' with result of '{result}' has been processed.");
                return new KeyValuePair<string, string>(queueItem, result);
            }
            catch (Exception exception)
            {
                log.LogError(exception.ToString());
                throw;
            }
        }
    }
}
