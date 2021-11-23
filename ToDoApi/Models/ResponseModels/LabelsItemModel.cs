using System.Collections.Generic;
using ToDoApi.Database.Models;

namespace TodoAPI.Models.ResponseModels
{
    public class LabelsItemModel
    {
        public TodoItems TodoItem { get; set; }
        public List<Labels> LabelsAssigned { get; set; }
    }
}
