using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models.UpdateModels
{
    public class UpdateTodoItemModel
    {
        [Required]
        public int ItemID { get; set; }

        [Required]
        public string ItemName { get; set; }
        
        [Required]
        public int Id { get; set; }
    }

}
