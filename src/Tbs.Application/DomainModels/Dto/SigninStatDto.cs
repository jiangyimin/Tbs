namespace Tbs.DomainModels.Dto
{
    public class SigninStatDto
    {        

        public string Worker { get; set; }

        public int DayCount1 { get; set; }
        public int LateCount1 { get; set; }

        public int LateTotal1 { get; set; }
        public int DayCount2 { get; set; }
        public int LateCount2 { get; set; }

        public int LateTotal2 { get; set; }
        public int DayCount3 { get; set; }
        public int LateCount3 { get; set; }

        public int LateTotal3 { get; set; }

        public SigninStatDto(string worker)
        {
            Worker = worker;
        }
    }
}

