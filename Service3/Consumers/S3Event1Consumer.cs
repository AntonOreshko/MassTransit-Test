using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class S3Event1Consumer: IConsumer<Event1>
    {
        public async Task Consume(ConsumeContext<Event1> context)
        {
            Console.WriteLine($"<Service3> received <{context.Message.Value}>");
        }
    }
}