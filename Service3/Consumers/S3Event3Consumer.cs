using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class S3Event3Consumer: IConsumer<Event3>
    {
        public async Task Consume(ConsumeContext<Event3> context)
        {
            Console.WriteLine($"<Service3> received <{context.Message.Value}>");
        }
    }
}