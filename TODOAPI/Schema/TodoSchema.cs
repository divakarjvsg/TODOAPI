using System;
using TodoAPI.Queries;
using ToDoApi.DataAccess.Repositories.Contracts;

namespace TodoAPI.Schema
{
    public class TodoSchema:GraphQL.Types.Schema
    {
        public TodoSchema(ITodoListRepository todoListRepository, IServiceProvider provider) : base(provider)
        {
            //Query = new TodoQuery(todoListRepository);
        }
    }
}
