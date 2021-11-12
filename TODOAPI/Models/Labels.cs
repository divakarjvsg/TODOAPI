using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class Labels
    {

        [Key]
        public int LabelId { get; set; }

        public string LabelName { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
