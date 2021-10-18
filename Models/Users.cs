using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Users
    {
        public Guid SourcedId { get; set; }
        public string Status { get; set; }
        public DateTime DateLastModified { get; set; }
        public string Metadata { get; set; }
        public string Username { get; set; }
        public bool EnabledUser { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
        public string Role { get; set; }
        public string Identifier { get; set; }
        public string Email { get; set; }
        public string QrCode { get; set; }
        public string IdPicPath { get; set; }

    }
}
