using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class IdTemplate
    {
        public int ID { get; set; }
        public string TemplateName { get; set; }
        public string FrontTopColor { get; set; }
        public string FrontMiddleColor { get; set; }
        public string FrontBottomColor { get; set; }
        public string BackTopColor { get; set; }
        public string BackMiddleColor { get; set; }
        public string BackBottomColor { get; set; }
        public byte[] Logo { get; set; }
        public string FrontTopLabel { get; set; }
        public string BackTopLabel { get; set; }
        public string BackInfoLineOne { get; set; }
        public string BackInfoLineTwo { get; set; }
        public string BackInfoLineThree { get; set; }

    }
}
