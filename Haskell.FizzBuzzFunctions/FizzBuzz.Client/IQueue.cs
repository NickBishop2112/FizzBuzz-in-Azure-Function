namespace FizzBuzz.Client
{
    using System.Threading.Tasks;

    public interface IQueue
    {
        Task WriteAsync(string v);

        Task<string> ReadAsync();
    }
}
