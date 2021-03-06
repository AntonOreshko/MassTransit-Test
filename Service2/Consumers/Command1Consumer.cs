﻿using System;
using System.Threading.Tasks;
using Common.Messages;
using MassTransit;

namespace Service2.Consumers
{
    public class Command1Consumer: IConsumer<Command1>
    {
        public async Task Consume(ConsumeContext<Command1> context)
        {
            Console.WriteLine($"<Service2> received <{context.Message.Value}>");   
        }
    }
}