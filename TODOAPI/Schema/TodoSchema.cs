using GraphQL;
using TodoAPI.Types;

namespace TodoAPI.Schema
{
    public class TodoSchema:GraphQL.Types.Schema
    {
        public TodoSchema(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
            Query = dependencyResolver.Resolve<TodoListTypes>();
        }
    }
}
