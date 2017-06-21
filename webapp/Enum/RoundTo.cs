using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum RoundTo
    {
        [Description("0 Minute")]
        Minute0 = 0,
        [Description("5 Minute")]
        Minute5 = 5,
        [Description("10 Minute")]
        Minute10 = 10,
        [Description("15 Minute")]
        Minute15 = 15,
        [Description("30 Minute")]
        Minute30 = 30
    }
}