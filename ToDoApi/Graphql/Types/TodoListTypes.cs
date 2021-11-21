using GraphQL.Types;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Types
{
    public class TodoListTypes:ObjectGraphType<TodoLists>
    {
        public TodoListTypes(ITodoItemsRepository todoItemsRepository)
        {
            Field(x => x.Id);
            Field(x => x.TodoListName);
            //Field(x => x.CreatedBy);
            Field(x => x.CreatedDateTime);
            //Field<ListGraphType<TodoItemType>>("todoItems",
            //    arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            //    resolve: context =>
            //    {
            //        var lastItemsFilter = context.GetArgument<int?>("id");
            //        return lastItemsFilter != null
            //            ? todoItemsRepository.GetTodoItemforListID(lastItemsFilter.Value)
            //            : todoItemsRepository.GetTodoItemforListID(context.Source.Id);
            //    });

        }
    }
}
