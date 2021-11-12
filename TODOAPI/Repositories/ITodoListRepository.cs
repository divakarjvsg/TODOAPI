using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;

namespace TodoAPI.Interfaces
{
    public interface ITodoListRepository
    {
        Task<IEnumerable<TodoLists>> Search(string TodoListName);
        Task<IEnumerable<TodoLists>> GetTodoLists(PageParmeters pageParmeters);
        Task<TodoLists> GetTodoList(int Id);
        Task<TodoLists> GetTodoListByName(string TodoListName);
        Task<TodoLists> AddTodoList(TodoLists todoList);
        Task<TodoLists> UpdateTodoList(TodoLists todoList);
        Task DeleteTodoList(int Id);
        Task<TodoLists> GetTodoListByGuid(Guid ItemGuid);
    }
}
