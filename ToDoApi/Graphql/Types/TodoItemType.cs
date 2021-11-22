using HotChocolate.Types;
using ToDoApi.Database.Models;

namespace TodoAPI.Types
{
    public class TodoItemType: ObjectType<TodoItems>
    {
        //public TodoItemType(ITodoItemsRepository todoItemsRepository)
        //{
        //    Field(x => x.ItemID);
        //    Field(x => x.ItemName);
        //    Field(x => x.CreatedBy);
        //    Field(x => x.CreatedDateTime);
        //    Field(x => x.Id);
        //}
    }
}
