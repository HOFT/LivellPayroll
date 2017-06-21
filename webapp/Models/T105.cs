using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class T105
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DateEndPeriod { get; set; }
        public DateTime DateStartPeriod { get; set; }
        public DateTime DateSubmitted { get; set; }
        [StringLength(64)]
        public string LName { get; set; }
        [StringLength(64)]
        public string FName { get; set; }
        [StringLength(64)]
        public string MInit { get; set; }
        [StringLength(11)]
        public string SSN { get; set; }
        public byte F99 { get; set; }
        [StringLength(256)]
        public string Address1 { get; set; }
        [StringLength(256)]
        public string Address2 { get; set; }
        [StringLength(64)]
        public string City { get; set; }
        [StringLength(16)]
        public string ZipCode { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [StringLength(16)]
        public string Phone { get; set; }
        public decimal F100 { get; set; }
        public decimal F101 { get; set; }
        public decimal F102 { get; set; }
        public DateTime F103 { get; set; }
        public DateTime F104 { get; set; }
        public DateTime F105 { get; set; }
        public float S100 { get; set; }
        public float S101 { get; set; }
        public float S102 { get; set; }
        public float S103 { get; set; }
        public decimal S104 { get; set; }
        public decimal S105 { get; set; }
        public decimal S106 { get; set; }
        public decimal S107 { get; set; }
        public decimal S108 { get; set; }
        public decimal S109 { get; set; }
        public decimal S110 { get; set; }
        public decimal S111 { get; set; }
        public decimal S112 { get; set; }
        public decimal S113 { get; set; }
        public decimal S114 { get; set; }
        public decimal S115 { get; set; }
        public float S116 { get; set; }
        public float S117 { get; set; }
        public decimal S118 { get; set; }
        public decimal S119 { get; set; }
        public decimal S120 { get; set; }
        public decimal S121 { get; set; }
        public decimal S122 { get; set; }
        [StringLength(10)]
        public string F106 { get; set; }
        public int F107 { get; set; }
        [StringLength(16)]
        public string F108 { get; set; }
        [StringLength(32)]
        public string F109 { get; set; }
        [StringLength(32)]
        public string F110 { get; set; }
        [StringLength(32)]
        public string F111 { get; set; }
        public int F112 { get; set; }
        public int F113 { get; set; }
        public byte F114 { get; set; }
        public byte F115 { get; set; }
        public byte F116 { get; set; }
        public byte F117 { get; set; }
        public byte F118 { get; set; }
        public byte F119 { get; set; }
        public float F120 { get; set; }
        public decimal F121 { get; set; }
        public decimal F122 { get; set; }
        [StringLength(64)]
        public string S1231 { get; set; }
        public float S1241 { get; set; }
        [StringLength(64)]
        public string S1232 { get; set; }
        public float S1242 { get; set; }


        [StringLength(64)]
        public string S1233 { get; set; }
        public float S1243 { get; set; }
        [StringLength(64)]
        public string S1234 { get; set; }
        public float S1244 { get; set; }
        [StringLength(64)]
        public string S1235 { get; set; }
        public float S1245 { get; set; }
        [StringLength(64)]
        public string S1251 { get; set; }
        public decimal S1261 { get; set; }
        [StringLength(64)]
        public string S1252 { get; set; }


        public decimal S1262 { get; set; }
        [StringLength(64)]
        public string S1253 { get; set; }
        public decimal S1263 { get; set; }
        [StringLength(64)]
        public string S1254 { get; set; }
        public decimal S1264 { get; set; }
        [StringLength(64)]
        public string S1255 { get; set; }
        public decimal S1265 { get; set; }
        [StringLength(64)]
        public string S127 { get; set; }
        public float F1231 { get; set; }
        public float F1232 { get; set; }
        public float F1233 { get; set; }
        public float F1234 { get; set; }
        public float F1235 { get; set; }
        public byte F124 { get; set; }
        public byte F125 { get; set; }
        public byte IsW2StatutoryEmployee { get; set; }
        public byte IsW2RetirementPlan { get; set; }
        public byte DoesReceiveAdvanceEIC { get; set; }
        public decimal EmployerSocialSecurity { get; set; }
        public decimal EmployerMedicare { get; set; }
        public decimal AdvanceEIC { get; set; }
        public int CheckNo { get; set; }
        public decimal DateLastEdit { get; set; }
        public byte Checked { get; set; }
        [StringLength(64)]
        public string s1236 { get; set; }
        public float s1246 { get; set; }
        [StringLength(64)]
        public string s1237 { get; set; }
        public float s1247 { get; set; }
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
        public byte IsPaid { get; set; }

        public byte is1099Employee { get; set; }
        public float PTOAcchours { get; set; }
        public float VacAccHours { get; set; }
        public byte printPTOStub { get; set; }
        public float PTOAccRate { get; set; }
        public float VacAccRate { get; set; }
        public float PTOCapHours { get; set; }
        public float VacCapHours { get; set; }
        public byte isPayrollSetup { get; set; }
        [StringLength(256)]
        public string Memo { get; set; }
        [Required]
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}