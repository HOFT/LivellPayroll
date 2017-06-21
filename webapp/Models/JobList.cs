using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class JobList
    {
        [StringLength(128)]
        public string CustomerName { get; set; }
        public List<Jobs> Jobs { get; set; }
    }
    public class Jobs {
        public int JobId { get; set; }
        [StringLength(256)]
        public string JobName { get; set; }
    }
}