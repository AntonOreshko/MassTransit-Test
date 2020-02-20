using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Options;

namespace Common.MassTransit.Services
{
    public interface IMassTransitService
    {
        Task PublishMessage<TMessage>(TMessage message) where TMessage : class;

        Task SendMessage<TMessage>(TMessage message) where TMessage : class;
    }

    public class MassTransitService : IMassTransitService
    {
        private readonly MassTransitSettings _massTransitSettings;

        private readonly IBusControl _serviceBus;
        
        public MassTransitService(IOptions<MassTransitSettings> massTransitSettings, IBusControl serviceBus)
        {
            _massTransitSettings = massTransitSettings.Value;
            _serviceBus = serviceBus;
        }

        public async Task PublishMessage<TMessage>(TMessage message) where TMessage : class
        {
            await _serviceBus.Publish(message);
        }

        public async Task SendMessage<TMessage>(TMessage message) where TMessage : class
        {
            var sendEndpoint = await _serviceBus.GetSendEndpoint(
                new Uri(_massTransitSettings.AzureServiceBusUrl + new object().TypeName<TMessage>()));
            
            await sendEndpoint.Send(message);
        }
    }
}