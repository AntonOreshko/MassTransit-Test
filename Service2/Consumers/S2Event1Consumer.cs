using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service2.Consumers
{
    public class S2Event1Consumer: IConsumer<Event1>
    {
        public async Task Consume(ConsumeContext<Event1> context)
        {
            Console.WriteLine($"<Service2> received <{context.Message.Value}>");
        }
    }
}