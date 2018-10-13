namespace Tbs.DomainModels.Dto
{
    public class RouteWorkerStatDto
    {        

        public string Worker { get; set; }

        public int DayCount { get; set; }
        public int RoleCount1 { get; set; }

        public int RoleCount2 { get; set; }
        public int RoleCount3 { get; set; }

        public int RoleCount4 { get; set; }
        public int RoleCount5 { get; set; }

        public int RoleCount6 { get; set; }

        public RouteWorkerStatDto(string worker)
        {
            Worker = worker;
        }
    }
}

