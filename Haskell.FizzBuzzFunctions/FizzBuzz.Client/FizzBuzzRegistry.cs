namespace FizzBuzz.Client
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using StructureMap;

    public class FizzBuzzRegistry : Registry
    {
        public FizzBuzzRegistry(IConfiguration configuration)
        {
            this.For<IQueueHandler>()
                .Use<QueueHandler>()
                .Ctor<CloudQueueClient>()
                .Is(() => CloudStorageAccount.Parse(configuration["Connection"]).CreateCloudQueueClient())
                .Ctor<string>("inputQueueName").Is(configuration["InputQueueName"])
                .Ctor<string>("outputQueueName").Is(configuration["OutputQueueName"]);

            this.For<IFizzBuzzClient>()
                .Use<FizzBuzzClient>()
                .Ctor<TextWriter>()
                .Is(() => Console.Out)
                .Ctor<IConfiguration>()
                .Is(configuration);
        }
    }
}
