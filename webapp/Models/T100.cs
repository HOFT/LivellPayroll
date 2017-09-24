using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class T100
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(128)]
        public string BankName { get; set; }
        [StringLength(256)]
        public string BankInfo1 { get; set; }
        [StringLength(256)]
        public string BankInfo2 { get; set; }
        [StringLength(256)]
        public string BankInfo3 { get; set; }
        [StringLength(64)]
        public string TransitCode { get; set; }
        [StringLength(64)]
        public string BankRouteNo { get; set; }
        [StringLength(64)]
        public string BankAccountNo { get; set; }
        public int StartCheckNo { get; set; }
        public int CurrentCheckNo { get; set; }
        public float CheckWidth { get; set; }
        public float CheckHeight { get; set; }
        public float OffsetLeft { get; set; }
        public float OffsetRight { get; set; }
        public float OffsetUp { get; set; }
        public float OffsetDown { get; set; }
        [StringLength(256)]
        public string Logo { get; set; }
        [StringLength(256)]
        public string Signature { get; set; }
        [StringLength(256)]
        public string Company1 { get; set; }
        [StringLength(256)]
        public string Company2 { get; set; }
        [StringLength(256)]
        public string Company3 { get; set; }
        [StringLength(256)]
        public string Company4 { get; set; }
        public bool BlankBankStock { get; set; }
        [StringLength(256)]
        public string ExField1 { get; set; }
        public bool ExField2 { get; set; }
        [StringLength(256)]
        public string ExField3 { get; set; }
        public bool nodisplaymicr { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}