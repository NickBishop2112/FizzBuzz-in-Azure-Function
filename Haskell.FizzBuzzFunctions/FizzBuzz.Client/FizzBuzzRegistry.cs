namespace FizzBuzz.Client
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using StructureMap;
    using System;
    using System.IO;

    public class FizzBuzzRegistry : Registry
    {
        public FizzBuzzRegistry(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.For<IQueue>()
                .Use<Queue>()
                .Ctor<CloudQueueClient>()
                .Is(() => CloudStorageAccount.Parse(configuration["Connection"]).CreateCloudQueueClient())
                .Ctor<string>("inputQueueName").Is(configuration["InputQueueName"])
                .Ctor<string>("outputQueueName").Is(configuration["OutputQueueName"]);

            this.For<IFizzBuzzClient>()
                .Use<FizzBuzzClient>()
                .Ctor<TextWriter>()
                .Is(() => Console.Out);
        }
    }
}
