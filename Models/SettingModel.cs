using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class SettingModel
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string SettingDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
