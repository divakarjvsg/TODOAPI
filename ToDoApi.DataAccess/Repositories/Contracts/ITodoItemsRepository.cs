using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories.Contracts
{
    public interface ITodoItemsRepository
    {
        Task<IEnumerable<TodoItems>> Search(string ItemName);
        Task<IEnumerable<TodoItems>> GetTodoItems(PageParmeters pageParmeters);
        Task<TodoItems> GetTodoItem(int ItemId);
        Task<TodoItems> GetTodoItemByName(string ItemName);
        Task<TodoItems> AddTodoItem(TodoItems todoItem);
        Task<TodoItems> UpdateTodoItem(TodoItems todoItem);
        Task DeleteTodoItem(int ItemId);
        Task<TodoItems> GetTodoItemByGuid(Guid ItemGuid);
        Task<IEnumerable<TodoItems>> GetTodoItemforListID(int ListId);
    }
}
