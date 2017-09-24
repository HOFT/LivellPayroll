using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class T108
    {
        public DateTime DateEndPeriod { get; set; }
        public DateTime DateStartPeriod { get; set; }
        public DateTime DateSubmitted { get; set; }
        public Guid EID { get; set; }
        [StringLength(64)]
        public string LName { get; set; }
        [StringLength(64)]
        public string FName { get; set; }
        [StringLength(64)]
        public string MInit { get; set; }
        [StringLength(11)]
        public string SSN { get; set; }
        public int F99 { get; set; }
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
        public float F100 { get; set; }
        public float F101 { get; set; }
        public float F102 { get; set; }
        public float F1002 { get; set; }
        public DateTime F103 { get; set; }
        public DateTime F104 { get; set; }
        public DateTime F105 { get; set; }
        public float S100 { get; set; }
        public float S101 { get; set; }
        public float S102 { get; set; }
        public float S1002 { get; set; }
        public float S103 { get; set; }
        public float S104 { get; set; }
        public float S105 { get; set; }
        public float S1052 { get; set; }
        public float S106 { get; set; }
        public float S107 { get; set; }
        public float S108 { get; set; }
        public float S109 { get; set; }
        public float S110 { get; set; }
        public float S111 { get; set; }
        public float S112 { get; set; }
        public float S113 { get; set; }
        public float S114 { get; set; }
        public float S115 { get; set; }
        public double S116 { get; set; }
        public double S117 { get; set; }
        public float S118 { get; set; }
        public float S119 { get; set; }
        public float S120 { get; set; }
        public float S121 { get; set; }
        public float S122 { get; set; }
        public string F106 { get; set; }
        public int F107 { get; set; }
        public string F108 { get; set; }
        public string F109 { get; set; }
        public string F110 { get; set; }
        public string F111 { get; set; }
        public int F112 { get; set; }
        public int F113 { get; set; }
        public int F114 { get; set; }
        public int F115 { get; set; }
        public int F116 { get; set; }
        public int F117 { get; set; }
        public int F118 { get; set; }
        public int F119 { get; set; }
        public double F120 { get; set; }
        public float F121 { get; set; }
        public float F122 { get; set; }
        public string S1231 { get; set; }
        public float S1241 { get; set; }
        public string S1232 { get; set; }
        public float S1242 { get; set; }
        public string S1233 { get; set; }
        public float S1243 { get; set; }
        public string S1234 { get; set; }
        public float S1244 { get; set; }
        public string S1235 { get; set; }
        public float S1245 { get; set; }
        public string S1251 { get; set; }
        public float S1261 { get; set; }
        public string S1252 { get; set; }
        public float S1262 { get; set; }
        public string S1253 { get; set; }
        public float S1263 { get; set; }
        public string S1254 { get; set; }
        public float S1264 { get; set; }
        public string S1255 { get; set; }
        public float S1265 { get; set; }
        public string S127 { get; set; }
        public double F1231 { get; set; }
        public double F1232 { get; set; }
        public double F1233 { get; set; }
        public double F1234 { get; set; }
        public double F1235 { get; set; }
        public int F124 { get; set; }
        public int F125 { get; set; }
        public int IsW2StatutoryEmployee { get; set; }
        public int IsW2RetirementPlan { get; set; }
        public int DoesReceiveAdvanceEIC { get; set; }
        public float EmployerSocialSecurity { get; set; }
        public float EmployerMedicare { get; set; }
        public float AdvanceEIC { get; set; }
        public int CheckNo { get; set; }
        public DateTime DateLastEdit { get; set; }
        public string S1236 { get; set; }
        public float S1246 { get; set; }
        public string S1237 { get; set; }
        public float S1247 { get; set; }
        public float F1236 { get; set; }
        public float F1237 { get; set; }
        public string HourlyPay1 { get; set; }
        public string HourlyPay2 { get; set; }
        public string HourlyPay3 { get; set; }
        public string HourlyPay4 { get; set; }
        public string HourlyPay5 { get; set; }
        public float SickRate { get; set; }
        public float VacationRate { get; set; }
        public int is1099Employee { get; set; }
        public float PTOAcchours { get; set; }
        public float VacAccHours { get; set; }
        public int printPTOStub { get; set; }
        public float PTOAccRate { get; set; }
        public float VacAccRate { get; set; }
        public double PTOCapHours { get; set; }
        public double VacCapHours { get; set; }
        public string Memo { get; set; }

    }
}