using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum Period
    {
        [Description("Weekly")]  
        Weekly = 1,
        [Description("Biweekly")] 
        Biweekly = 2,
        [Description("Daily")]  
        Daily = 3,
        [Description("Semimonthly")]  
        Semimonthly = 4,
        [Description("Monthly")]   
        Monthly = 5
    }
}