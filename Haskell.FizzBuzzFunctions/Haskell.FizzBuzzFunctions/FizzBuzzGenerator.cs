namespace FizzBuzz.Application
{
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    public static class FizzBuzzGenerator
    {
        [FunctionName("Generate")]
        public static string Generate([QueueTrigger("FizzBuzz-messages-001", Connection = "")]int number, ILogger log)
        {
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
    }
}
