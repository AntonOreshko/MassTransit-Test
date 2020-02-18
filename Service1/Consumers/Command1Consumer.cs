using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service1.Consumers
{
    public class Command1Consumer: IConsumer<Command1>
    {
        public async Task Consume(ConsumeContext<Command1> context)
        {
            Console.WriteLine($"<Service1> received <{context.Message.Value}>");   
        }
    }
}