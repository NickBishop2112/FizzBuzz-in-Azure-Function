namespace FizzBuzz.Client
{
    using Microsoft.Extensions.Configuration;
    using StructureMap;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var configuration =
                new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", true, true)
                  .AddUserSecrets<Program>()
                  .Build();

            var container = new Container();
            container.Configure(context =>
            {
                context.AddRegistry(new FizzBuzzRegistry(configuration));                
            });

            container.GetInstance<IFizzBuzzClient>().Show();
            Console.ReadKey();
        }
    }
}
