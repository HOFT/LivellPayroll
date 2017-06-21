using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class TS_Info
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public string Note { get; set; }
        public bool Paid { get; set; }
        public DateTime TimeSheetDate { get; set; }
        public int TimeSheetType { get; set; }
        public double TotalWorkTime { get; set; }
        public bool Locked { get; set; }
        public string JobName { get; set; }
    }
}