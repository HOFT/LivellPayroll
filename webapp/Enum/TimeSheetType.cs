using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum TimeSheetType
    {
        [Description("Regular Work")]  
        RegularWork = 1,
        [Description("Sick Time")]  
        SickTime = 2,
        [Description("Vacation")]  
        Vacation = 3
    }
}