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
    public class Organizations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [JsonProperty("sourcedId")]
        [Display(Name = "Sourced Id")]
        public Guid OrgSourcedId { get; set; }
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
        [JsonProperty("name")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        [Display(Name = "Type")]
        public string Type { get; set; }
        [JsonProperty("identifier")]
        [Display(Name = "Identifier")]
        public string Identifier { get; set; }
        [JsonProperty("parent")]
        [Display(Name = "Parent Org")]
        [ForeignKey("Parent_SourcedId")]
        public Organizations Parent { get; set; }
        public List<Users> Users { get; set; }
        public List<Classes> Classes { get; set; }
        public List<Courses> Courses { get; set; }
        public Guid? Parent_SourcedId { get; set; }

    }
}
