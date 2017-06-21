using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class T102
    {
        public Guid Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        [StringLength(16)]
        public string CodeMap { get; set; }
        [Required]
        [StringLength(64)]
        public string Description { get; set; }
        public double AnnualLimit { get; set; }
        [DefaultValue(false)]
        public bool Taxable { get; set; }
        [DefaultValue(false)]
        public bool FICATaxable { get; set; }
        [DefaultValue(false)]
        public bool PctofIncome { get; set; }
        [DefaultValue(false)]
        public bool Enabled { get; set; }
        [DefaultValue(false)]
        public bool W2Box10 { get; set; }
        [DefaultValue(false)]
        public bool W2Box12 { get; set; }
        [StringLength(64)]
        public string W2Code { get; set; }
        [StringLength(64)]
        public string ExField1 { get; set; }
        public int ExField2 { get; set; }
        [StringLength(64)]
        public string ExField3 { get; set; }
        public int ExField4 { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}