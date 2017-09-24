using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Enum
{
    public enum Status
    {
        [Description("Active")]  //激活
        Active = 0,
        [Description("Inactive")]  //不活跃
        Inactive =1,
        [Description("Freeze ")]  //冻结
        Freeze = 2,
        [Description("Pending")]   //待激活
        Pending = 3,
    }
}