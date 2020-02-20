using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service2.Consumers
{
    public class Request1Consumer: IConsumer<Request1>
    {
        public async Task Consume(ConsumeContext<Request1> context)
        {
            Console.WriteLine($"<Service2> received request <{context.Message.Value}>");

            await context.RespondAsync(new Response1());
        }
    }
}