using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class AccrualTimeJournal
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string SSN { get; set; }
        [StringLength(255)]
        public string EmployeeName { get; set; }
        public int Type { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        [StringLength(255)]
        public string Memo { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}