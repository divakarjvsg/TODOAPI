using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories.Contracts
{
    public interface ITodoItemsRepository
    {
        Task<IEnumerable<TodoItems>> Search(string itemName);
        Task<IEnumerable<TodoItems>> GetTodoItems(PageParmeters pageParmeters);
        Task<TodoItems> GetTodoItem(int itemId);
        Task<TodoItems> GetTodoItemByName(string itemName);
        Task<TodoItems> AddTodoItem(TodoItems todoItem);
        Task<TodoItems> UpdateTodoItem(TodoItems todoItem);
        Task DeleteTodoItem(int itemId);
        Task<TodoItems> GetTodoItemByGuid(Guid itemGuid);
        Task<IEnumerable<TodoItems>> GetTodoItemforListID(int listId);
    }
}
