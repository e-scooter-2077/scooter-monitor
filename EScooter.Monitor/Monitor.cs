using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Infrastructure.Events.ServiceBus;
using EasyDesk.CleanArchitecture.Infrastructure.Json;
using EasyDesk.CleanArchitecture.Infrastructure.Time;
using EScooter.Monitor.Web.DTO;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EScooter.Monitor
{
    public static class Monitor
    {
        public const string ServiceBusVariableName = "ServiceBusConnectionString";

        [Function("scooter-monitor")]
        public static async Task Run([ServiceBusTrigger("%ReportedPropertiesTopicName%", "%ReportedPropertiesSubscriptionName%", Connection = ServiceBusVariableName)] string mySbMsg, IDictionary<string, object> userProperties, FunctionContext context)
        {
            var logger = context.GetLogger(typeof(Monitor).Name);
            logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var scooterDeviceTwin = JsonConvert.DeserializeObject<Twin>(mySbMsg, new TwinJsonConverter());
            var reportedProps = scooterDeviceTwin.Properties.Reported;

            var connectionString = Environment.GetEnvironmentVariable(ServiceBusVariableName);
            var serviceBusClient = new ServiceBusClient(connectionString);
            var topicDescriptor = AzureServiceBusSenderDescriptor.Topic(Environment.GetEnvironmentVariable("ScooterStatusTopicName"));
            var eventBusPublisher = new AzureServiceBusPublisher(serviceBusClient, topicDescriptor);
            var serializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            var externalEventsPublisher = new ExternalEventPublisher(eventBusPublisher, new MachineDateTime(), serializer);
            var scooterStatus = new ScooterStatus();
            await externalEventsPublisher.Publish(scooterStatus);
            return;
        }
    }
}
