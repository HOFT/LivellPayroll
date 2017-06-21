using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(128)]
        [Required]
        public string CustomerName { get; set; }
        [StringLength(128)]
        public string Attn { get; set; }
        [StringLength(16)]
        public string Telphone { get; set; }
        [StringLength(16)]
        public string Fax { set; get; }  
        [StringLength(128)]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(256)]
        public string Address { get; set; }
        [StringLength(256)]
        public string Remark { get; set; }
        public DateTime AddDate { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Job> Job { get; set; }

    }
}