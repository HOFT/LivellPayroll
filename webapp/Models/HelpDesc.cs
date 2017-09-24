using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LivellPayRoll.Models
{
    public class HelpDesc
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(512)]
        [DefaultValue("")]
        public string Title { get; set; }
        [StringLength(2)]
        public string Type { get; set; }
        [StringLength(512)]
        [DefaultValue("")]
        public string Keyword { get; set; }
        [StringLength(512)]
        public string Path { get; set; }
        [StringLength(1024)]
        [DefaultValue("")]
        public string Content { get; set; }
    }
}