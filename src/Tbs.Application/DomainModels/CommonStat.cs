using System;
using System.Collections.Generic;

namespace Tbs.DomainModels
{
    internal class CommonStat
    {
        public HashSet<DateTime> DtHS {get; set; }
        public int[] Qtums { get; set; }

        public CommonStat(int count)
        {
            DtHS = new HashSet<DateTime>();
            Qtums = new int[count];
        }
    }
}