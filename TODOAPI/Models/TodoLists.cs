using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class TodoLists
    {
        public int Id { get; set; }

        [Required]
        public string TodoListName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        
        [Required]
        public Guid ListGuid { get; set; }
        
        [Required]
        public Guid CreatedBy { get; set; }
        public List<TodoItems> todoItems { get; set; }        
    }
}
