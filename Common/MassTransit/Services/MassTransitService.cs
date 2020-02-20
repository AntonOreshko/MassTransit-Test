using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.MassTransit.Services
{
    public interface IMassTransitService
    {
        Task PublishMessage<TMessage>(TMessage message) where TMessage : class;

        Task SendMessage<TMessage>(TMessage message) where TMessage : class;

        Task<TResponse> Request<TRequest, TResponse>(TRequest requestMessage) where TRequest : class where TResponse : class;
    }

    public class MassTransitService : IMassTransitService
    {
        private readonly MassTransitSettings _massTransitSettings;

        private readonly IBusControl _serviceBus;
        
        private readonly IServiceProvider _serviceProvider;

        public MassTransitService(IOptions<MassTransitSettings> massTransitSettings, IBusControl serviceBus, IServiceProvider serviceProvider)
        {
            _massTransitSettings = massTransitSettings.Value;
            _serviceBus = serviceBus;
            _serviceProvider = serviceProvider;
        }

        public async Task PublishMessage<TMessage>(TMessage message) where TMessage : class
        {
            await _serviceBus.Publish(message);
        }

        public async Task SendMessage<TMessage>(TMessage message) where TMessage : class
        {
            var sendEndpoint = await _serviceBus.GetSendEndpoint(new Uri(_massTransitSettings.AzureServiceBusUrl + this.TypeName<TMessage>()));
            
            await sendEndpoint.Send(message);
        }

        public async Task<TResponse> Request<TRequest, TResponse>(TRequest requestMessage) where TRequest : class where TResponse : class
        {
            var request = _serviceProvider.GetRequiredService<IRequestClient<TRequest, TResponse>>();

            var response = await request.Request(requestMessage);

            return response;
        }
    }
}