using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Interfaces;

namespace TodoAPI.Types
{
    public class TodoItemType: ObjectGraphType<TodoAPI.Models.TodoItems>
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
