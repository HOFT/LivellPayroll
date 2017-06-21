using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class TS_Edit
    {
        public string Employee { get; set; }
        public string Job { get; set; }
        public double StartDate { get; set; }
        public double StopDate { get; set; }
        public int TimeSheetType { get; set; }
        public double TotalWorkTime { get; set; }
        public string Note { get; set; }
        public bool Paid { get; set; }
    }
}