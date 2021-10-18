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
        public Guid SourcedID { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ParentSourcedID { get; set; }
        public int SchoolYear { get; set; }
    }
}
