using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Subjects
    {
        [Key]
        public Guid SourcedId { get; set; }
        public string Title { get; set; }

    }
}
