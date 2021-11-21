using GraphQL.Types;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Types
{
    public class TodoItemType: ObjectGraphType<TodoItems>
    {
        public TodoItemType(ITodoItemsRepository todoItemsRepository)
        {
            Field(x => x.ItemID);
            Field(x => x.ItemName);
            Field(x => x.CreatedBy);
            Field(x => x.CreatedDateTime);
            Field(x => x.Id);
        }
    }
}
