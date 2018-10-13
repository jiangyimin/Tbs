using System;
using Abp.Dependency;

namespace Tbs.Timing
{
    public class TimeUtil
    {
        /// <summary>
        /// Gets the startup time of the application.
        /// </summary>
        public static int DistToNow(DateTime theDay, string theTime)
        {
            int hour = int.Parse(theTime.Substring(0, 2));
            int minute = int.Parse(theTime.Substring(3, 2));
            DateTime dt = new DateTime(theDay.Year, theDay.Month, theDay.Day, hour, minute, 0);

            TimeSpan ts = DateTime.Now.Subtract(dt);
            return (int)ts.TotalMinutes;
        }

        public static bool IsIn(DateTime theDay, string start, string end)
        {
            int s = DistToNow(theDay, start);
            int t = DistToNow(theDay, end);

            if (s >=0 && t <= 0) return true;
            
            return false;    
        }
    }
}
