using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models.UpdateModels
{
    public class UpdateTodoItemModel
    {
        [Required]
        public int ItemID { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public int ListId { get; set; }
    }
}
