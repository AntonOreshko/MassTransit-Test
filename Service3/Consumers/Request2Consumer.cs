using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class Request2Consumer: IConsumer<Request2>
    {
        public async Task Consume(ConsumeContext<Request2> context)
        {
            Console.WriteLine($"<Service3> received request <{context.Message.Value}>");

            await context.RespondAsync(new Response2());
        }
    }
}