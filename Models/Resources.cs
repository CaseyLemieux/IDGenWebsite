using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Resources
    {
        public Guid SourcedId { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public string Metadata { get; set; }
        public string Title { get; set; }
        public string Importance  { get; set; }
        public string VendorResourceId { get; set; }
        public string VendorId { get; set; }
        public string ApplicationId { get; set; }
    }
}
