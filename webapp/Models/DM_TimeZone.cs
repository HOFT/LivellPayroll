using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class DM_TimeZone
    {
        [Key]
        public int id { set; get; }
        [StringLength(8)]
        public string Code { set; get; }
        [StringLength(128)]
        public string TimeZone { set; get; }
    }
}