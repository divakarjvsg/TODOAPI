using GraphQL.Types;
using TodoAPI.Interfaces;
using TodoAPI.Types;

namespace TodoAPI.Queries
{
    public class TodoQuery:ObjectGraphType
    {
        public TodoQuery(ITodoListRepository todoListRepository)
        {

            Field<ListGraphType<TodoListTypes>>(
                "AlltodoList",
                resolve: context => todoListRepository.Search("Monday"));

            Field<TodoListTypes>(
                "TodoList",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => todoListRepository.GetTodoList(context.GetArgument<int>("id")));
        }
    }
}
