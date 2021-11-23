using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Database.Models
{
    public class TodoLists
    {
        public int Id { get; set; }
        [Required]
        public string TodoListName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        [Required]
        public Guid ListGuid { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        public ICollection<TodoItems> TodoItems { get; set; }
    }
}
