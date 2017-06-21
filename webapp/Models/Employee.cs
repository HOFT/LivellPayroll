using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; }
        [StringLength(64)]
        public string LName { get; set; }
        [StringLength(64)]
        public string FName { get; set; }
        [StringLength(64)]
        public string MInit { get; set; }
        [StringLength(11)]
        public string SSN { get; set; }
        [DefaultValue(0)]
        [Required]
        public byte F99 { get; set; }
        [StringLength(256)]
        public string Address1 { get; set; }
        [StringLength(256)]
        public string Address2 { get; set; }
        [StringLength(64)]
        public string City { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(16)]
        public string ZipCode { get; set; }
        [StringLength(8)]
        public string TimeZone { get; set; }
        [StringLength(16)]
        public string Phone { get; set; }
        [StringLength(128)]
        [EmailAddress]
        public string Email { set; get; }
        public decimal F100 { get; set; }
        public decimal F101 { get; set; }
        public decimal F102 { get; set; }
        public DateTime F103 { get; set; }
        public DateTime F104 { get; set; }
        public DateTime F105 { get; set; }
        [StringLength(16)]
        public string F106 { get; set; }
        public int F107 { get; set; }
        [StringLength(16)]
        public string F108 { get; set; }
        [StringLength(16)]
        public string F109 { get; set; }
        [StringLength(16)]
        public string F110 { get; set; }
        [StringLength(16)]
        public string F111 { get; set; }
        public int F112 { get; set; }
        public int F113 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F114 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F115 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F116 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F117 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F118 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F119 { get; set; }
        public float F120 { get; set; }
        public decimal F121 { get; set; }
        public decimal F122 { get; set; }
        public float F1231 { get; set; }
        public float F1232 { get; set; }
        public float F1233 { get; set; }
        public float F1234 { get; set; }
        public float F1235 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F124 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte F125 { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte IsW2StatutoryEmployee { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte IsW2RetirementPlan { get; set; }
        [Required]
        [DefaultValue(0)]
        public byte DoesReceiveAdvanceEIC { get; set; }
        public float F1236 { get; set; }
        public float F1237 { get; set; }
        [StringLength(64)]
        public string HourlyPay1 { get; set; }
        [StringLength(64)]
        public string HourlyPay2 { get; set; }
        [StringLength(64)]
        public string HourlyPay3 { get; set; }
        [StringLength(64)]
        public string HourlyPay4 { get; set; }
        public decimal SickRate { get; set; }
        public decimal VacationRate { get; set; }
        [DefaultValue(0)]
        public byte is1099Employee { get; set; }
        public float PTOAcchours { get; set; }
        public float VacAccHours { get; set; }
        [DefaultValue(0)]
        public byte printPTOStub { get; set; }
        public float PTOAccRate { get; set; }
        public float VacAccRate { get; set; }
        public float PTOCapHours { get; set; }
        public float VacCapHours { get; set; }
        [DefaultValue(0)]
        public byte isPayrollSetup { get; set; }
        public int DefaultJob { get; set; }
        [DefaultValue(0)]
        public byte isEmailComfirmed { get; set; }
        [StringLength(128)]
        public string SecurityStamp { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<TimeSheet> TimeSheet { get; set; }
        public virtual ICollection<Job> Job { get; set; }
        public virtual ICollection<T105> T105 { get; set; }
        public virtual AppUser AppUser { get; set; }

    }
}