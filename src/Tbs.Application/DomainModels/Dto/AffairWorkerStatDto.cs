namespace Tbs.DomainModels.Dto
{
    public class AffairWorkerStatDto
    {        

        public string Worker { get; set; }

        public string Name { get; set; }

        public int DayCount { get; set; }
        public int RoleCount1 { get; set; }

        public int RoleCount2 { get; set; }
        public int RoleCount3 { get; set; }
        public int RoleCount4 { get; set; }

        public int NoCheckInCount { get; set; }
        public int NoCheckOutCount { get; set; }
        
        public AffairWorkerStatDto(string worker)
        {
            Worker = worker;
        }
    }
}

