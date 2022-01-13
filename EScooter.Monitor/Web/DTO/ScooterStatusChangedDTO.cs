using EasyDesk.CleanArchitecture.Application.Events.ExternalEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EScooter.Monitor.Web.DTO
{
    public record ScooterStatusChangedDTO(
        string Id,
        bool? Locked,
        string UpdateFrequency,
        double? MaxSpeed,
        bool? Standby) : ExternalEvent;
}
