namespace FizzBuzz.Client
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQueue
    {
        Task WriteAsync(string v);

        Task<IDictionary<string, string>> ReadAsync();
    }
}
