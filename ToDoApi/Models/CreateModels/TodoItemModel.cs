using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class TodoItemModel
    {
        [Required]
        public string ItemName { get; set; }
        [Required]
        public int ListId { get; set; }
    }
}
