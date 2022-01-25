using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EScooter.Monitor.Web.DTO
{
    public record ReportedProperties(
        bool? Locked,
        string UpdateFrequency,
        double? MaxSpeed,
        bool? Standby);
}
