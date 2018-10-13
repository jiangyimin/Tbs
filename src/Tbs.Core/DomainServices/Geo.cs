﻿using System;

namespace Tbs.DomainServices
{
    public class Geo
    {
        private const double EARTH_RADIUS = 6378.137 * 1000;//地球半径,单位为米  
        private static double rad(double d)
        {  
            return d* Math.PI / 180.0;  
        }  
        /// <summary>  
        /// 返回两点之间的距离，单位为米  
        /// </summary>  
        /// <param name="lat1"></param>  
        /// <param name="lng1"></param>  
        /// <param name="lat2"></param>  
        /// <param name="lng2"></param>  
        /// <returns></returns>  
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {  
            double radLat1 = rad(lat1);  
            double radLat2 = rad(lat2);  
            double a = radLat1 - radLat2;  
            double b = rad(lng1) - rad(lng2);  
 
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));  
            s = s* EARTH_RADIUS;  
            s = Math.Round(s* 10000) / 10000;  
            return s;  
        }      
    }
}
