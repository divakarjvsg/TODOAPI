using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Queries
{
    public class TodoQuery
    {
        private readonly ITodoListRepository todoListRepository;
        private readonly ITodoItemsRepository todoItemsRepository;
        private readonly ILabelRepository labelRepository;
        private readonly PageParmeters pageParmeters;
        public TodoQuery([Service]ITodoListRepository todoListRepository, [Service]ITodoItemsRepository todoItemsRepository, [Service]ILabelRepository labelRepository)
        {
            pageParmeters = new PageParmeters();
            this.todoListRepository = todoListRepository;
            this.todoItemsRepository = todoItemsRepository;
            this.labelRepository = labelRepository;
        }

        public async Task<IEnumerable<Labels>> GetAllLabels()
        {
            return await labelRepository.GetLabels(pageParmeters);
        }

        public async Task<Labels> GetLabelById(int labelId)
        {
            return await labelRepository.GetLabel(labelId);
        }

        public async Task<IEnumerable<TodoItems>> GetAllToDoItems()
        {
            return await todoItemsRepository.GetTodoItems(pageParmeters);
        }

        public async Task<TodoItems> GetToDoItemById(int ItemId)
        {
            return await todoItemsRepository.GetTodoItem(ItemId);
        }

        public async Task<IEnumerable<TodoLists>> GetAllToDoLists()
        {            
            return await todoListRepository.GetTodoLists(pageParmeters);
        }

        public async Task<TodoLists> GetToDoListById(int ListId)
        {
            return await todoListRepository.GetTodoList(ListId);
        }
    }
}
