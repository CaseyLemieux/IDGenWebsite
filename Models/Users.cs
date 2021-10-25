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
    public class Users
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
        public string Username { get; set; }
        public bool EnabledUser { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }
        public string Identifier { get; set; }
        public string Email { get; set; }
        public string Sms { get; set; }
        public string Phone { get; set; }
        public string QrCode { get; set; }
        public string IdPicPath { get; set; }
        public List<Organizations> Organizations { get; set; }
        public List<Grades> Grades { get; set; }

    }
}
