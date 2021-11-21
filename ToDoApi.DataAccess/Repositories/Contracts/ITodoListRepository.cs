using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories.Contracts
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
