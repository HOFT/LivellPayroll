using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        [StringLength(128)]
        [Required]
        public string CompanyName { get; set; }
        [StringLength(32)]
        public string FedTaxId { get; set; }
        [StringLength(64)]
        public string ContactName { get; set; }  //联络人（注册用户的管理员）
        [StringLength(16)]   
        public string Telphone { set; get; }  //联系电话
        [StringLength(16)]
        public string Fax { set; get; }  //联系电话
        [StringLength(128)]
        [EmailAddress]
        [Required]
        public string Email { set; get; }  //公司邮箱
        [StringLength(32)]
        public string WWW { set; get; }
        [StringLength(256)]  
        [Required]
        public string Address1 { set; get; }
        [StringLength(256)]
        public string Address2 { set; get; }
        [StringLength(64)]
        [Required]
        public string City { set; get; }
        [StringLength(2)]
        [Required]
        public string State { set; get; }
        [StringLength(64)]
        public string Country { set; get; }
        [StringLength(16)]
        public string Zip { set; get; }
        [StringLength(64)]
        public string TradeName { set; get; }
        [StringLength(2)]
        [Required]
        public string PayFreq { set; get; }    //Daily Or XXX
        [StringLength(8)]
        [Required]
        public string TimeZone { set; get; }    //时区
        [StringLength(2)]
        [Required]
        public string RoundTo { set; get; }    //近似时间值
        [Required]
        public bool WeekRule { set; get; }
        [Required]
        public double WeekRuleValue { set; get; }
        [Required]
        public bool DayRule { set; get; }
        [Required]
        public double DayRuleValue { set; get; }
        [Required]
        public bool DoubeRule { set; get; }
        [Required]
        public double DoubeRuleValue { set; get; }
        [Required]
        public bool CaliforniaRule { set; get; }
        [Required]
        public bool allowedit { set; get; }
        [Required]
        public bool PayReportByEndingDate { set; get; }    //True Or False
        [StringLength(64)]
        public string ControlNo { set; get; }
        [StringLength(64)]
        public string Establish { set; get; }
        public double FUTA { set; get; }
        [StringLength(64)]
        public string StateID { set; get; }
        public double StateUnemWage { set; get; }
        public double SUTA { set; get; }
        [Required]   
        public DateTime PayRollRegTime { set; get; }  //系统注册时间
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Job> Job { get; set; }
        public virtual ICollection<T201> T201 { get; set; }
        public virtual ICollection<TimeSheet> TimeSheet { get; set; }
        public virtual ICollection<T102> T102 { get; set; }
        public virtual ICollection<T102> T105 { get; set; }
        public virtual ICollection<AppUser> AppUser { get; set; }
    }
}