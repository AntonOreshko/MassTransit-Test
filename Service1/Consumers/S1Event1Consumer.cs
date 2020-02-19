using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service1.Consumers
{
    public class S1Event1Consumer: IConsumer<Event1>, IConsumer<Event2>, IConsumer<Event3>
    {
        public async Task Consume(ConsumeContext<Event1> context)
        {
            Console.WriteLine($"<Service1> S1 received <{context.Message.Value}>");
        }

        public async Task Consume(ConsumeContext<Event2> context)
        {
            Console.WriteLine($"<Service1> S1 received <{context.Message.Value}>");
        }

        public async Task Consume(ConsumeContext<Event3> context)
        {
            Console.WriteLine($"<Service1> S1 received <{context.Message.Value}>");
        }
    }
}