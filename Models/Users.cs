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
        [JsonProperty("sourcedId")]
        [Display(Name = "Source Id")]
        public Guid UserSourcedId { get; set; }
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
        [JsonProperty("username")]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [JsonProperty("enabledUser")]
        [Display(Name = "Enabled")]
        public bool EnabledUser { get; set; }
        [JsonProperty("givenName")]
        [Display(Name = "Given name")]
        public string GivenName { get; set; }
        [JsonProperty("familyName")]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }
        [JsonProperty("middleName")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [JsonProperty("role")]
        [Display(Name = "Role")]
        public string Role { get; set; }
        [Display(Name = "Identifier")]
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [JsonProperty("sms")]
        [Display(Name = "SMS")]
        public string Sms { get; set; }
        [JsonProperty("phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Display(Name = "Qr Code")]
        public string QrCode { get; set; }
        [Display(Name = "ID Picture")]
        public string IdPicPath { get; set; }
        [JsonProperty("orgs")]
        public List<Organizations> Organizations { get; set; }
        [JsonProperty("grades")]
        [NotMapped]
        public List<string> StringGrades { get; set; }
        public List<Grades> Grades { get; set; }
        public List<Enrollments> Enrollments { get; set; }

    }
}
