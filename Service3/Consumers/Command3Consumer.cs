using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class Command3Consumer: IConsumer<Command1>
    {
        public async Task Consume(ConsumeContext<Command1> context)
        {
            Console.WriteLine($"<Service3> received <{context.Message.Value}>");
        }
    }
}