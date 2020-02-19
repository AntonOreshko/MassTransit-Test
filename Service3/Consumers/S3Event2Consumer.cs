using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class S3Event2Consumer: IConsumer<Event2>
    {
        public async Task Consume(ConsumeContext<Event2> context)
        {
            Console.WriteLine($"<Service3> received <{context.Message.Value}>");
        }
    }
}