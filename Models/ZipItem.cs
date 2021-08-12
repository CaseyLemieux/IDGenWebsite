using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class ZipItem
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }

        public ZipItem(string name, byte[] content)
        {
            this.Name = name;
            this.Content = content;
        }

    }
}
