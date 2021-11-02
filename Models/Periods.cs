using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IDGenWebsite.Models
{
    public class Periods
    {
        [Key]
        public Guid PeriodSourcedId { get; set; }
        public string Title { get; set; }
    }
}
