namespace FizzBuzz.Client
{
    using System.Threading.Tasks;

    public interface IFizzBuzzClient
    {
        void ShowAsync(int minimum, int maximum);
    }
}