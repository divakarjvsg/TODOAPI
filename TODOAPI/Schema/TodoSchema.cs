using System;
using ToDoApi.DataAccess.Repositories.Contracts;

namespace TodoAPI.Schema
{
    public class TodoSchema
    {
        public TodoSchema(ITodoListRepository todoListRepository, IServiceProvider provider)// : base(provider)
        {
            //Query = new TodoQuery(todoListRepository);
        }
    }
}
