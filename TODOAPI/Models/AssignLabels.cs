using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    
    public class AssignLabels 
    {
        [Key]
        public int AssignId { get; set; }
        public int LabelId { get; set; }

        public Guid AssignedGuid { get; set; }
    }
}
