using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Database.Models
{
    public class Labels
    {
        [Key]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
