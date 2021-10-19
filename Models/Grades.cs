using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Grades
    {
        [Key]
        public Guid SourcedId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Courses Courses { get; set; }
        public Classes Classes { get; set; }

    }
}
