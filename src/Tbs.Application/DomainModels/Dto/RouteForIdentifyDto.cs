using System.Collections.Generic;

namespace Tbs.DomainModels.Dto
{
    public class RotueForIdentifyDto
    {
        public string SubWorkerCn { get; set; }

        public List<RouteTaskDto> Tasks { get; set; }
   }
}