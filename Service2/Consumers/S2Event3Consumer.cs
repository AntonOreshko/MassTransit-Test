using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service2.Consumers
{
    public class S2Event3Consumer: IConsumer<Event3>
    {
        public async Task Consume(ConsumeContext<Event3> context)
        {
            Console.WriteLine($"<Service2> received <{context.Message.Value}>");
        }
    }
}