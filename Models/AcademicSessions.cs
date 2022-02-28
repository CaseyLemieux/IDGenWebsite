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
    public class AcademicSessions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [JsonProperty("sourcedId")]
        [Display(Name = "Session Sourced Id")]
        public Guid SessionSourcedId { get; set; }
        [JsonProperty("status")]
        [Display(Name = "Status")]
        public string Status { get; set; }
        [JsonProperty("dateLasteModified")]
        [Display(Name = "Date Last Modified")]
        public DateTime DateLastModified { get; set; }
        public List<Metadatas> Metadata { get; set; }
        [JsonProperty("metadata")]
        [NotMapped]
        public JObject TempMetadata { get; set; }
        [JsonProperty("title")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        [Display(Name = "Type")]
        public string Type { get; set; }
        [Display(Name = "Start Date")]
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [JsonProperty("parent")]
        [Display(Name = "Parent Sourced Id")]
        [ForeignKey("Parent_SourcedId")]
        public AcademicSessions Parent { get; set; }
        [JsonProperty("schoolYear")]
        [Display(Name = "School Year")]
        public string SchoolYear { get; set; }
        public List<Classes> Classes { get; set; }
        //public Guid? Parent_SourcedId { get; set; }

    }
}
