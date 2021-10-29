using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Courses
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid SourcedId { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public List<Metadatas> Metadata { get; set; }
        [JsonProperty("metadata")]
        [NotMapped]
        public JObject TempMetadata { get; set; }
        public string Title { get; set; }
        public AcademicSessions SchoolYear { get; set; }
        public string CourseCode { get; set; }
        public List<Grades> Grades { get; set; }
        public List<Subjects> Subjects { get; set; }
        public Organizations Organization { get; set; }
        public List<SubjectCodes> SubjectCodes { get; set; }
        public List<Resources> Resources { get; set; }


    }
}
