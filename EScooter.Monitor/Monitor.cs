using Azure.Messaging.ServiceBus;
using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using EasyDesk.CleanArchitecture.Infrastructure.Events.ServiceBus;
using EasyDesk.CleanArchitecture.Infrastructure.Json;
using EasyDesk.CleanArchitecture.Infrastructure.Time;
using EasyDesk.Tools.Options;
using EScooter.Monitor.Web.DTO;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EasyDesk.Tools.Options.OptionImports;

namespace EScooter.Monitor
{
    public static class Monitor
    {
        public const string ServiceBusVariableName = "ServiceBusConnectionString";

        [Function("scooter-monitor")]
        public static async Task Run([ServiceBusTrigger("%ReportedPropertiesTopicName%", "%ReportedPropertiesSubscriptionName%", Connection = ServiceBusVariableName)] string mySbMsg, IDictionary<string, object> userProperties, FunctionContext context)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var logger = context.GetLogger(typeof(Monitor).Name);
            logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            userProperties.TryGetValue("deviceId", out object scooterId);
            var id = scooterId.ToString();
            var res = CreateStatusEventFromTriggerMessage(id, mySbMsg, serializerSettings);

            var connectionString = Environment.GetEnvironmentVariable(ServiceBusVariableName);
            var serviceBusClient = new ServiceBusClient(connectionString);
            var topicDescriptor = AzureServiceBusSenderDescriptor.Topic(Environment.GetEnvironmentVariable("ScooterStatusTopicName"));
            var eventBusPublisher = new AzureServiceBusPublisher(serviceBusClient, topicDescriptor);
            var serializer = new NewtonsoftJsonSerializer(serializerSettings);
            var externalEventsPublisher = new ExternalEventPublisher(eventBusPublisher, new MachineDateTime(), serializer);

            await res.IfPresentAsync(async x => await externalEventsPublisher.Publish(x));
            return;
        }

        public static Option<ScooterStatusChangedDTO> CreateStatusEventFromTriggerMessage(string id, string msg, JsonSerializerSettings serializerSettings)
        {
            var scooterDeviceTwin = JsonConvert.DeserializeObject<Twin>(msg, new TwinJsonConverter());
            var reportedProps = scooterDeviceTwin.Properties.Reported;
            var reportedPropsObj = JsonConvert.DeserializeObject<ReportedPropertiesDTO>(reportedProps.ToJson(), serializerSettings);
            if (reportedPropsObj is { Locked: null, MaxSpeed: null, Standby: null, UpdateFrequency: null })
            {
                return None;
            }
            var scooterStatus = new ScooterStatusChangedDTO(
                Id: id,
                Locked: reportedPropsObj.Locked,
                UpdateFrequency: reportedPropsObj.UpdateFrequency,
                MaxSpeed: reportedPropsObj.MaxSpeed,
                Standby: reportedPropsObj.Standby);
            return Some(scooterStatus);
        }
    }
}
