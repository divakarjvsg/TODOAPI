using GraphQL;
using GraphQL.Types;
using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Types;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Queries
{
    public class TodoQuery:ObjectGraphType
    {
        private readonly ITodoListRepository todoListRepository;
        private readonly ITodoItemsRepository todoItemsRepository;
        private readonly ILabelRepository labelRepository;

        public TodoQuery([Service]ITodoListRepository todoListRepository, [Service]ITodoItemsRepository todoItemsRepository, [Service]ILabelRepository labelRepository)
        {

            //Field<ListGraphType<TodoListTypes>>(
            //    "AlltodoList",
            //    resolve: context => todoListRepository.Search("Monday"));

            //Field<TodoListTypes>(
            //    "TodoList",
            //    arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
            //    resolve: context => todoListRepository.GetTodoList(context.GetArgument<int>("id")));

            this.todoListRepository = todoListRepository;
            this.todoItemsRepository = todoItemsRepository;
            this.labelRepository = labelRepository;
        }

        //public async Task<List<Labels>> GetAllLabels()
        //{
        //    return await labelRepository.GetLabels();
        //}

        public async Task<Labels> GetLabelById(int labelId)
        {
            return await labelRepository.GetLabel(labelId);
        }

        //public async Task<List<TodoItems>> GetAllToDoItems()
        //{
        //    return await todoItemsRepository.GetTodoItems();
        //}

        public async Task<TodoItems> GetToDoItemById(int ItemId)
        {
            return await todoItemsRepository.GetTodoItem(ItemId);
        }
        //public async Task<List<TodoLists>> GetAllToDoLists()
        //{
        //    return await todoListRepository.GetTodoLists();
        //}

        public async Task<TodoLists> GetToDoListById(int ListId)
        {
            return await todoListRepository.GetTodoList(ListId);
        }
    }
}
