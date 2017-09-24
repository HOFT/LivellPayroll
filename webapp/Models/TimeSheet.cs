using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class TimeSheet
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime StopDate { get; set; }
        [StringLength(512)]
        public string Note { get; set; }
        public bool Paid { get; set; }
        public DateTime TimeSheetDate { get; set; }
        public int TimeSheetType { get; set; }
        [DefaultValue(0)]
        public double VacationTime { get; set; }
        [DefaultValue(0)]
        public double TotalWorkTime { get; set; }
        [DefaultValue(0)]
        public double SickTime { get; set; }
        [DefaultValue(0)]
        public double RegulaWorkTime { get; set; }
        [DefaultValue(0)]
        public double OverTimeWorkTime { get; set; }
        [DefaultValue(0)]
        public double DoubleWorkTime { get; set; }
        public float RegularPayrate { get; set; }
        public float OvertimePayrate { get; set; }
        public float DoublePayrate { get; set; }
        [StringLength(2)]
        [DefaultValue("1")]
        public string Status { get; set; }
        [DefaultValue(0)]
        public bool ExtraInt1 { get; set; }
        [DefaultValue(0)]
        public bool ExtraInt2 { get; set; }
        [StringLength(256)]
        public string ExtraStr1 { get; set; }
        [StringLength(256)]
        public string ExtraStr2 { get; set; }
        [Required]
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}