using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Classes
    {
        public Guid SourcedID { get; set; }
        public string Status { get; set; }
        public string DateLastModified { get; set; }
        public string Title { get; set; }
        public Guid CourseSourcedID { get; set; }
        public string ClassCode { get; set; }
        public string ClassType { get; set; }
        public string Location { get; set; }
        public Guid SchoolSourcedID { get; set; }
        public Guid TermSourcedID { get; set; }
    }
}
