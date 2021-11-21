using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Database.Models
{
    public class AssignLabels
    {
        [Key]
        public int AssignId { get; set; }
        public int LabelId { get; set; }
        public Guid AssignedGuid { get; set; }
    }
}
