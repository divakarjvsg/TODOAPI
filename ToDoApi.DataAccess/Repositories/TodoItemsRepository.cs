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
        public TodoItemsRepository(AppDbContext appDbContext
            , IHttpContextAccessor httpContextAccessor
            )
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

        public async Task DeleteTodoItem(int ItemId)
        {
            var result = await _appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemID == ItemId && x.CreatedBy == _loginUser);
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

        public async Task<TodoItems> GetTodoItem(int ItemId)
        {
            return await _appDbContext.TodoItems
               .FirstOrDefaultAsync(y => y.ItemID == ItemId && y.CreatedBy == _loginUser);
        }

        public async Task<TodoItems> GetTodoItemByGuid(Guid ItemGuid)
        {
            return await _appDbContext.TodoItems
               .FirstOrDefaultAsync(y => y.ItemGuid == ItemGuid);
        }
        public async Task<TodoItems> GetTodoItemByName(string ItemName)
        {
            return await _appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemName == ItemName && x.CreatedBy == _loginUser);
        }

        public async Task<IEnumerable<TodoItems>> GetTodoItemforListID(int ListId)
        {
            IQueryable<TodoItems> query = _appDbContext.TodoItems.Where(x => x.Id == ListId);
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TodoItems>> GetTodoItems(PageParmeters pageParmeters)
        {
            IQueryable<TodoItems> query = _appDbContext.TodoItems.Where(x => x.CreatedBy == _loginUser);
            var result = await query.Skip((pageParmeters.PageNumber - 1) * pageParmeters.PageSize).Take(pageParmeters.PageSize).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TodoItems>> Search(string ItemName)
        {
            IQueryable<TodoItems> query = _appDbContext.TodoItems.Where(x => x.CreatedBy == _loginUser);
            if (!string.IsNullOrEmpty(ItemName))
            {
                query = query.Where(x => x.ItemName.Contains(ItemName));
            }
            return await query.ToListAsync();
        }

        public async Task<TodoItems> UpdateTodoItem(TodoItems todoItem)
        {
            var result = await _appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemID == todoItem.ItemID);

            if (result != null)
            {
                result.ModifiedDateTime = DateTime.Now;

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
