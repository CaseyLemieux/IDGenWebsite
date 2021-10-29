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
    public class Enrollments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [JsonProperty("sourcedId")]
        public Guid SourcedId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("dateLastModified")]
        public DateTime DateLastModified { get; set; }
        public List<Metadatas> Metadata { get; set; }
        [JsonProperty("metadata")]
        [NotMapped]
        public JObject TempMetadata { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }
        [ForeignKey("ClassId")]
        public Classes Class { get; set; }
        [ForeignKey("SchoolId")]
        public Organizations School { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("primary")]
        public bool Primary { get; set; }
        [JsonProperty("beginDate")]
        public DateTime BeginDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }



        public string UserId { get; set; }
        public string ClassId { get; set; }
        public string SchoolId { get; set; }

    }
}
