namespace FizzBuzz.Client
{
    using Microsoft.Extensions.Configuration;
    using StructureMap;
    using System;
    using System.IO;

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var configuration =
                    new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddCommandLine(args)
                      .AddJsonFile("appsettings.json", true, true)
                      .AddUserSecrets<Program>()
                      .Build();

                var container = new Container();
                container.Configure(context =>
                {
                    context.AddRegistry(new FizzBuzzRegistry(configuration));
                });

                container.GetInstance<IFizzBuzzClient>()
                    .ShowAsync(
                        configuration.GetValue<int>("min"),
                        configuration.GetValue<int>("max"))
                        .GetAwaiter()
                        .GetResult();
            }
            catch (Exception exception)
            {
                Console.Error.Write(exception);
            }
        }
    }
}
