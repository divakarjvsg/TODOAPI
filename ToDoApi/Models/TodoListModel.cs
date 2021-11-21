using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class TodoListModel
    {
        [Required]
        public string TodoListName { get; set; }
    }
}
