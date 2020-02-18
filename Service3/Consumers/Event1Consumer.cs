﻿using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service3.Consumers
{
    public class Event1Consumer: IConsumer<Event1>
    {
        public async Task Consume(ConsumeContext<Event1> context)
        {
            Console.WriteLine($"<Service1> received <{context.Message.Value}>");
        }
    }
}