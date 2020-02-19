using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service2.Consumers
{
    public class S2Event2Consumer: IConsumer<Event2>
    {
        public async Task Consume(ConsumeContext<Event2> context)
        {
            Console.WriteLine($"<Service2> received <{context.Message.Value}>");
        }
    }
}