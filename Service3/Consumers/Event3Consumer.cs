using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class Event3Consumer: IConsumer<Event3>
    {
        public async Task Consume(ConsumeContext<Event3> context)
        {
            Console.WriteLine($"<Service1> received <{context.Message.Value}>");
        }
    }
}