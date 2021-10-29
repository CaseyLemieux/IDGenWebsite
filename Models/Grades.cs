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
        [Display(Name = "Source Id")]
        public Guid SourcedId { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Code")]
        public string Code { get; set; }
        public Courses Courses { get; set; }
        public Classes Classes { get; set; }
        [NotMapped]
        [Display(Name = "Ceds Grade Levels")]
        public Dictionary<string, string> CedsLevels = new Dictionary<string, string>()
        {
            {"IT", "Infant Todler" },
            {"PR", "Preschool" },
            {"PK", "Prekindergarten" },
            {"TK", "Transitional Kindergarten" },
            {"KG", "Kindergarten" },
            {"01", "First Grade" },
            {"02", "Second Grade" },
            {"03", "Thrid Grade" },
            {"04", "Fourth Grade" },
            {"05", "Fifth Grade" },
            {"06", "Sixth Grade" },
            {"07", "Seventh Grade" },
            {"08", "Eigth Grade" },
            {"09", "Ninth Grade" },
            {"10", "Tenth Grade" },
            {"11", "Eleventh Grade" },
            {"12", "Twelfth Grade" },
            {"13", "Grade 13" },
            {"PS", "Postsecondary" },
            {"UG", "Ungraded" },
            {"Other", "Other" }
        };
    }
}
