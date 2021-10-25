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
        public Guid SourcedId { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public List<Metadata> Metadata { get; set; }
        [JsonProperty("metadata")]
        [NotMapped]
        public JObject TempMetadata { get; set; }
        public Users User { get; set; }
        public Classes Class { get; set; }
        public Organizations School { get; set; }
        public string Role { get; set; }
        public bool Primary { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
