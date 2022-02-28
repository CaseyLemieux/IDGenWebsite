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
        [JsonProperty("sourcedId")]
        [Display(Name = "Course Sourced Id")]
        public Guid CourseSourcedId { get; set; }
        [JsonProperty("status")]
        [Display(Name = "Status")]
        public string Status { get; set; }
        [JsonProperty("dateLastModified")]
        [Display(Name = "Date Last Modified")]
        public DateTime DateLastModified { get; set; }
        public List<Metadatas> Metadata { get; set; }
        [JsonProperty("metadata")]
        [NotMapped]
        public JObject TempMetadata { get; set; }
        [JsonProperty("title")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [JsonProperty("schoolYear")]
        [Display(Name = "School Year")]
        public AcademicSessions SchoolYear { get; set; }
        [JsonProperty("courseCode")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        [JsonProperty("grades")]
        [NotMapped]
        public List<string> StringGrades { get; set; }
        public List<Grades> Grades { get; set; }
        [JsonProperty("subjects")]
        [NotMapped]
        public List<string> StringSubjects { get; set; }
        public List<Subjects> Subjects { get; set; }
        [JsonProperty("org")]
        [Display(Name = "Organizatin")]
        public Organizations Organization { get; set; }
        [JsonProperty("subjectCodes")]
        [NotMapped]
        public List<string> StringSubjectCodes { get; set; }
        public List<SubjectCodes> SubjectCodes { get; set; }
        [JsonProperty("resources")]
        public List<Resources> Resources { get; set; }

        //public Guid SessionSourcedId { get; set; }
        //public Guid OrgSourcedId { get; set; }


    }
}
