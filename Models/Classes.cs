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
    public class Classes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [JsonProperty("sourcedId")]
        [Display(Name = "Class Sourced Id")]
        public Guid ClassSourcedId { get; set; }
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
        [JsonProperty("classCode")]
        [Display(Name = "Class Code")]
        public string ClassCode { get; set; }
        [JsonProperty("classType")]
        [Display(Name = "Class Type")]
        public string ClassType { get; set; }
        [JsonProperty("location")]
        [Display(Name = "Location")]
        public string Location { get; set; }
        public List<Grades> Grades { get; set; }
        public List<Subjects> Subjects { get; set; }
        [JsonProperty("course")]
        [Display(Name = "Course")]
        public Courses Course { get; set; }
        [JsonProperty("school")]
        [Display(Name = "School")]
        public Organizations School { get; set; }
        [JsonProperty("terms")]
        public List<AcademicSessions> AcademicSessions { get; set; }
        public List<SubjectCodes> SubjectCodes { get; set; }
        public List<Periods> Periods { get; set; }
        public List<Resources> Resources { get; set; }
        [JsonProperty("subjects")]
        [NotMapped]
        public List<string> StringSubjects { get; set; }
        [JsonProperty("grades")]
        [NotMapped]
        public List<string> StringGrades { get; set; }
        [JsonProperty("periods")]
        [NotMapped]
        public List<string> StringPeriods { get; set; }
        [JsonProperty("subjectCodes")]
        [NotMapped]
        public List<string> StringSubjectCodes { get; set; }
        public List<Enrollments> Enrollments { get; set; }

        //public Guid OrgSourcedId { get; set; }
        //public Guid CourseSourcedId { get; set; }
    }
}
