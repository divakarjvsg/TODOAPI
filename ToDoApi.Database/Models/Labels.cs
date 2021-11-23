using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Database.Models
{
    public class Labels
    {
        /// <summary>
        /// labelId
        /// </summary>
        /// <example>1</example>
        [Key]
        public int LabelId { get; set; }
        /// <summary>
        /// LabelName
        /// </summary>
        /// <example>TestLabel</example>
        public string LabelName { get; set; }
        /// <summary>
        /// Created Guid
        /// </summary>
        /// <example>3f14083e-c50b-4051-a445-18cee883323f</example>
        public Guid? CreatedBy { get; set; }
    }
}
