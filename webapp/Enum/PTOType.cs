using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum PTOType
    {
        [Description("Paid Time Off (PTO)")]
        PTOAccRate = 1,
        [Description("Paid Vacation")]
        VacAccRate = 2
    }
}