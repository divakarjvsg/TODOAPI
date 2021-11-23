using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using TodoAPI.Graphql;
using TodoAPI.Queries;

namespace TodoAPI.Utilities
{
    public static class GraphQLServiceExtension
    {
        public static IServiceCollection AddGraphQLServices(this IServiceCollection service)
        {
            return service.AddGraphQL(s => SchemaBuilder.New()
                    .AddServices(s)
                    .AddQueryType<TodoQuery>()
                    .AddMutationType<TodoMutation>()
                    .Create());
        }
    }
}
