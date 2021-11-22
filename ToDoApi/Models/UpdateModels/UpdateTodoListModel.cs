using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.UpdateModels
{
    public class UpdateTodoListModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string TodoListName { get; set; }
    }
}
