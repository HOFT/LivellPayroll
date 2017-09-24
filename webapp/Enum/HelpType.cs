using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum HelpType
    {
        [Description("Everything")]
        Everything =0,
        [Description("Home")]
        Home = 1,
        [Description("Employee")]
        Employee = 2,
        [Description("Customer")]
        Customer = 3,
        [Description("TimeSheets")]
        TimeSheets = 4,
        [Description("Reports")]
        Reports = 5,
        [Description("Payroll")]
        Payroll = 6,
        [Description("Settings")]
        Settings = 7
    }
}