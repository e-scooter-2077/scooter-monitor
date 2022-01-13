using EasyDesk.Tools.Options;
using EScooter.Monitor.Web.DTO;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EScooter.Monitor.UnitTests
{
    public class CreateStatusEventFromTriggerMessageTest
    {
        private Twin Twin()
        {
            var t = new Twin();
            t.DeviceId = null;
            t.Properties.Reported = new TwinCollection();
            t.Properties.Reported["Locked"] = null;
            t.Properties.Reported["MaxSpeed"] = null;
            t.Properties.Reported["Standby"] = null;
            t.Properties.Reported["UpdateFrequency"] = null;

            return t;
        }

        private bool _locked = true;
        private double _maxSpeed = 10;
        private bool _standby = false;
        private string _updateFrequency = "00:00:30";

        private Twin CompleteTwin()
        {
            var t = new Twin();
            t.DeviceId = "___asddsas_";
            t.Properties.Reported["Locked"] = _locked;
            t.Properties.Reported["MaxSpeed"] = _maxSpeed;
            t.Properties.Reported["Standby"] = _standby;
            t.Properties.Reported["UpdateFrequency"] = _updateFrequency;
            return t;
        }

        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        [Fact]
        public void ShouldReturnNondeWithNullFields()
        {
            var t = Twin();
            Monitor.CreateStatusEventFromTriggerMessage(t.DeviceId, JsonConvert.SerializeObject(t, new TwinJsonConverter()), _serializerSettings)
                .ShouldBe(OptionImports.None);
        }

        [Fact]
        public void ShouldReturnCorrectFieldsWithCorrectInput()
        {
            var twin = CompleteTwin();
            Monitor.CreateStatusEventFromTriggerMessage(twin.DeviceId, JsonConvert.SerializeObject(twin, new TwinJsonConverter()), _serializerSettings)
                .ShouldBe(new ScooterStatusChangedDTO(
                    Id: twin.DeviceId,
                    Locked: _locked,
                    UpdateFrequency: _updateFrequency,
                    Standby: _standby,
                    MaxSpeed: _maxSpeed));
        }
    }
}
