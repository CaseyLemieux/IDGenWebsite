using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Classes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid SourcedId { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public string Metadata { get; set; }
        public string Title { get; set; }
        public string ClassCode { get; set; }
        public string ClassType { get; set; }
        public string Location { get; set; }
        public List<Grades> Grades { get; set; }
        public List<Subjects> Subjects { get; set; }
        public Courses Course { get; set; }
        public Organizations School { get; set; }
        public List<AcademicSessions> AcademicSessions { get; set; }
        public List<SubjectCodes> SubjectCodes { get; set; }
        public List<Periods> Periods { get; set; }
        public List<Resources> Resources { get; set; }
    }
}
