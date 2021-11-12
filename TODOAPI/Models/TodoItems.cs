using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class TodoItems
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        public string ItemName { get; set; }


        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        [Required]
        public Guid ItemGuid { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public int Id { get; set; }
        public TodoLists todoLists { get; set; }
    }
}
