namespace FizzBuzz.Client
{
    using Microsoft.Extensions.Configuration;
    using StructureMap;
    using System;

    class Program
    {
        static void Main(string[] args)
        {          
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
