using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoApi.DataAccess.Repositories.Contracts;
using ToDoApi.Database.Context;
using ToDoApi.Database.Models;

namespace ToDoApi.DataAccess.Repositories
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly Guid _loginUser;

        public TodoItemsRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            if (httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                _loginUser = new Guid(claims.ToList()[0].Value);
            }
        }

        public async Task<TodoItems> AddTodoItem(TodoItems todoItem)
        {
            todoItem.CreatedDateTime = DateTime.Now;
            todoItem.ItemGuid = Guid.NewGuid();
            todoItem.CreatedBy = _loginUser;
            var result = await _appDbContext.TodoItems.AddAsync(todoItem);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteTodoItem(int itemId)
        {
            var result = await _appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemID == itemId && x.CreatedBy == _loginUser);
            if (result != null)
            {
                var assignedresult = await _appDbContext.AssignLabels.Where(x => x.AssignedGuid == result.ItemGuid).ToListAsync();
                foreach (var assignedlabel in assignedresult)
                {
                    _appDbContext.AssignLabels.Remove(assignedlabel);
                }
                _appDbContext.TodoItems.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<TodoItems> GetTodoItem(int itemId)
        {
            return await _appDbContext.TodoItems
               .FirstOrDefaultAsync(y => y.ItemID == itemId && y.CreatedBy == _loginUser);
        }

        public async Task<TodoItems> GetTodoItemByGuid(Guid itemGuid)
        {
            return await _appDbContext.TodoItems
               .FirstOrDefaultAsync(y => y.ItemGuid == itemGuid);
        }

        public async Task<TodoItems> GetTodoItemByName(string itemName)
        {
            return await _appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemName == itemName && x.CreatedBy == _loginUser);
        }

        public async Task<IEnumerable<TodoItems>> GetTodoItemforListID(int listId)
        {
            IQueryable<TodoItems> query = _appDbContext.TodoItems.Where(x => x.Id == listId);
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TodoItems>> GetTodoItems(PageParmeters pageParmeters)
        {
            IQueryable<TodoItems> query = _appDbContext.TodoItems.Where(x => x.CreatedBy == _loginUser);
            var result = await query.Skip((pageParmeters.PageNumber - 1) * pageParmeters.PageSize).Take(pageParmeters.PageSize).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TodoItems>> Search(string itemName)
        {
            IQueryable<TodoItems> query = _appDbContext.TodoItems.Where(x => x.CreatedBy == _loginUser);
            if (!string.IsNullOrEmpty(itemName))
            {
                query = query.Where(x => x.ItemName.Contains(itemName));
            }
            return await query.ToListAsync();
        }

        public async Task<TodoItems> UpdateTodoItem(TodoItems todoItem)
        {
            var result = await _appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemID == todoItem.ItemID);

            if (result != null)
            {
                result.UpdatedDateTime = DateTime.Now;
                result.ItemName = todoItem.ItemName;

                if (todoItem.Id != 0)
                {
                    result.Id = todoItem.Id;
                }
                await _appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
