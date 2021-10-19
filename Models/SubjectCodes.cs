using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class SubjectCodes
    {
        [Key]
        public Guid SourcedId { get; set; }
        public string SubjectCode { get; set; }
        public string Title { get; set; }
    }
}
