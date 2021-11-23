using System.Collections.Generic;
using ToDoApi.Database.Models;

namespace TodoAPI.Models.ResponseModels
{
    public class LabelsListModel
    {
        public TodoLists TodoList { get; set; }
        public List<Labels> LabelsAssigned { get; set; }
    }
}
