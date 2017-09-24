using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum TimeSheetStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Passing")]
        Passing = 2,
        [Description("Locked")]
        Locked = 3
    }
}