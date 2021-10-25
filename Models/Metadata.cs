using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Metadata
    {
        [Key]
        public Guid SourcedId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
