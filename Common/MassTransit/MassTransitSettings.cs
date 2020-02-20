namespace Common.MassTransit
{
    public class MassTransitSettings
    {
        public string AzureServiceBusConnectionString { get; set; }

        public string AzureServiceBusUrl { get; set; }
        
        public double RequestTimeout { get; set; }
        
        public double RequestTimeToLive { get; set; }
    }
}