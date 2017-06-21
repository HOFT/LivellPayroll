using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }
        [StringLength(256)]
        [Required]
        public string JobName { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
        [StringLength(2)]
        [Required]
        public string  status { get; set; }   //最大雇员人数
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<TimeSheet> TimeSheet { get; set; }
    }
}