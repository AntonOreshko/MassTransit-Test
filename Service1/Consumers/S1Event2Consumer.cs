﻿using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service1.Consumers
{
    public class S1Event2Consumer: IConsumer<Event2>
    {
        public async Task Consume(ConsumeContext<Event2> context)
        {
            Console.WriteLine($"<Service1> received <{context.Message.Value}>");
        }
    }
}