using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class T201
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int  Ord { get; set; }
        [Required]
        [StringLength(16)]
        public string CodeMap { get; set; }
        [StringLength(64)]
        public string Description { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}