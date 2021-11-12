using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoAPI.Context;
using TodoAPI.Interfaces;
using TodoAPI.Models;

namespace TodoAPI.Repositories
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid LoginUser;
        public TodoItemsRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            LoginUser = new Guid(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public async Task<TodoItems> AddTodoItem(TodoItems todoItem)
        {
            todoItem.CreatedDateTime = DateTime.Now;
            todoItem.ItemGuid = Guid.NewGuid();
            todoItem.CreatedBy = LoginUser;
            var result = await appDbContext.TodoItems.AddAsync(todoItem);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteTodoItem(int ItemId)
        {
            var result = await appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemID == ItemId && x.CreatedBy==LoginUser);

            if (result != null)
            {
                appDbContext.TodoItems.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<TodoItems> GetTodoItem(int ItemId)
        {
            return await appDbContext.TodoItems
               .FirstOrDefaultAsync(y => y.ItemID == ItemId && y.CreatedBy == LoginUser);
        }

        public async Task<TodoItems> GetTodoItemByGuid(Guid ItemGuid)
        {
            return await appDbContext.TodoItems
               .FirstOrDefaultAsync(y => y.ItemGuid == ItemGuid);
        }
        public async Task<TodoItems> GetTodoItemByName(string ItemName)
        {
            return await appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemName == ItemName && x.CreatedBy == LoginUser);
        }

        public async Task<IEnumerable<TodoItems>> GetTodoItems(PageParmeters pageParmeters)
        {
            IQueryable<TodoItems> query = appDbContext.TodoItems.Where(x => x.CreatedBy == LoginUser);

            var result = await query.Skip((pageParmeters.pageNumber - 1) * pageParmeters.pageSize).Take(pageParmeters.pageSize).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TodoItems>> Search(string ItemName)
        {

            IQueryable<TodoItems> query = appDbContext.TodoItems.Where(x => x.CreatedBy == LoginUser);

            if (!string.IsNullOrEmpty(ItemName))
            {
                query = query.Where(x => x.ItemName.Contains(ItemName));
            }
                        
            return await query.ToListAsync();
        }

        public async Task<TodoItems> UpdateTodoItem(TodoItems todoItem)
        {
            var result = await appDbContext.TodoItems
                .FirstOrDefaultAsync(x => x.ItemID == todoItem.ItemID);

            if (result != null)
            {
                result.ModifiedDateTime = DateTime.Now;

                result.ItemName = todoItem.ItemName;
                               
                if (todoItem.Id != 0)
                {
                    result.Id = todoItem.Id;
                }
             
                await appDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
