using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Models;

namespace TodoAPI.Queries
{
    public class TodoQuery
    {
        private readonly ITodoListsRepository _todoListRepository;
        private readonly ITodoItemsRepository _todoItemsRepository;
        private readonly ILabelsRepository _labelRepository;
        private readonly PageParmeters _pageParmeters;

        public TodoQuery([Service] ITodoListsRepository todoListRepository, [Service] ITodoItemsRepository todoItemsRepository, [Service] ILabelsRepository labelRepository)
        {
            _pageParmeters = new PageParmeters();
            _todoListRepository = todoListRepository;
            _todoItemsRepository = todoItemsRepository;
            _labelRepository = labelRepository;
        }

        public async Task<IEnumerable<Labels>> GetAllLabels()
        {
            return await _labelRepository.GetLabels(_pageParmeters);
        }

        public async Task<Labels> GetLabelById(int labelId)
        {
            return await _labelRepository.GetLabel(labelId);
        }

        public async Task<IEnumerable<TodoItems>> GetAllToDoItems()
        {
            return await _todoItemsRepository.GetTodoItems(_pageParmeters);
        }

        public async Task<TodoItems> GetToDoItemById(int itemId)
        {
            return await _todoItemsRepository.GetTodoItem(itemId);
        }

        public async Task<IEnumerable<TodoLists>> GetAllToDoLists()
        {
            return await _todoListRepository.GetTodoLists(_pageParmeters);
        }

        public async Task<TodoLists> GetToDoListById(int listId)
        {
            return await _todoListRepository.GetTodoList(listId);
        }
    }
}
