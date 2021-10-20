using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class AcademicSessions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid SourcedId { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AcademicSessions Parent { get; set; }
        public string SchoolYear { get; set; }
        public List<Classes> Classes { get; set; }

    }
}
