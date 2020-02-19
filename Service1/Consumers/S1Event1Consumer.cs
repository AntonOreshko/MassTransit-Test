using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service1.Consumers
{
    public class S1Event1Consumer: IConsumer<Event1>
    {
        public async Task Consume(ConsumeContext<Event1> context)
        {
            Console.WriteLine($"<Service1> received <{context.Message.Value}>");
        }
    }
}