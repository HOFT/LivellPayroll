using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.App_Helpers
{
    public class TimeHelper
    {
        public static DateTime GetLocalTime(DateTime UTCtime,double TimeZoneCode)
        {  
            double TimeInt = ConvertDateTimeInt(UTCtime) + TimeZoneCode*3600;
            return ConvertIntDateTime(TimeInt);
        }
        public static DateTime GetUTCTime(DateTime LocalTime, double TimeZoneCode)
        {
            double TimeInt = ConvertDateTimeInt(LocalTime) - TimeZoneCode*3600;
            return ConvertIntDateTime(TimeInt);
        }

        public static DateTime ConvertIntDateTime(double d)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public static double ConvertDateTimeInt(DateTime time)
        {
            double intResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
    }

}