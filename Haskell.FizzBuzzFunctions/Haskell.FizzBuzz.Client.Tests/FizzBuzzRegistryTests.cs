﻿namespace FizzBuzz.Client.Tests
{
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using StructureMap;

    [TestClass]
    public class FizzBuzzRegistryTests
    {
        [TestMethod]
        public void IsFizzBuzzClientFound()
        {
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x["Connection"]).Returns("DefaultEndpointsProtocol=https;AccountName=storagequeuehaskell;AccountKey=IiSyYS3cs5sWCFPQhjQ4qqRJEsI3AFdwxpDeqmGn4idL+4iSsiVBTEQ7mPc+4CoLDT6t/f7dmOLDmXJMEJ4N9g==;BlobEndpoint=https://storagequeuehaskell.blob.core.windows.net/;QueueEndpoint=https://storagequeuehaskell.queue.core.windows.net/;TableEndpoint=https://storagequeuehaskell.table.core.windows.net/;FileEndpoint=https://storagequeuehaskell.file.core.windows.net/;");
            configuration.SetupGet(x => x["InputQueueName"]).Returns("input");
            configuration.SetupGet(x => x["OutputQueueName"]).Returns("output");

            using (var container = new Container(new FizzBuzzRegistry(configuration.Object)))
            {
                container
                    .GetInstance<IFizzBuzzClient>()
                    .Should()
                    .BeOfType<FizzBuzzClient>();
            }
        }
    }
}
