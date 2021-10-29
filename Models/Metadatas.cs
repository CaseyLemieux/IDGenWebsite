using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Metadatas
    {
        [Key]
        [Display(Name = "Source Id")]
        public Guid SourcedId { get; set; }
        [Display(Name = "Title")]
        public string Key { get; set; }
        [Display(Name = "Value")]
        public string Value { get; set; }
    }
}
