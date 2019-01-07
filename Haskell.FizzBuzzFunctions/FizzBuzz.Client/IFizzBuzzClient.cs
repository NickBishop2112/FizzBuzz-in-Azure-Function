namespace FizzBuzz.Client
{
    using System.Threading.Tasks;

    public interface IFizzBuzzClient
    {
        Task ShowAsync(int minimum, int maximum);
    }
}