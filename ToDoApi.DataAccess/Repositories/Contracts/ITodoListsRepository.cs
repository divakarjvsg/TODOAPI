using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories.Contracts
{
    public interface ITodoListsRepository
    {
        Task<IEnumerable<TodoLists>> Search(string todoListName);
        Task<IEnumerable<TodoLists>> GetTodoLists(PageParmeters pageParmeters);
        Task<TodoLists> GetTodoList(int listId);
        Task<TodoLists> GetTodoListByName(string todoListName);
        Task<TodoLists> AddTodoList(TodoLists todoList);
        Task<TodoLists> UpdateTodoList(TodoLists todoList);
        Task DeleteTodoList(int listId);
        Task<TodoLists> GetTodoListByGuid(Guid itemGuid);
    }
}
