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
    public class TodoListRepository : ITodoListRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid LoginUser;
        public TodoListRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            LoginUser = new Guid(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        public async Task<TodoLists> AddTodoList(TodoLists TodoListitem)
        {
            TodoListitem.CreatedDateTime = DateTime.Now;
            TodoListitem.ListGuid = Guid.NewGuid();
            TodoListitem.CreatedBy = LoginUser;
            var result = await appDbContext.TodoLists.AddAsync(TodoListitem);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteTodoList(int Id)
        {
            var result = await appDbContext.TodoLists
                .FirstOrDefaultAsync(x => x.Id == Id && x.CreatedBy == LoginUser);

            if (result != null)
            {
                appDbContext.TodoLists.Remove(result);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<TodoLists> GetTodoList(int Id)
        {
            return await appDbContext.TodoLists
                  .FirstOrDefaultAsync(y => y.Id == Id && y.CreatedBy == LoginUser);
        }

        public async Task<TodoLists> GetTodoListByName(string ListName)
        {
            return await appDbContext.TodoLists
                .FirstOrDefaultAsync(x => x.TodoListName == ListName && x.CreatedBy == LoginUser);
        }

        public async Task<TodoLists> GetTodoListByGuid(Guid ListGuid)
        {
            return await appDbContext.TodoLists
                .FirstOrDefaultAsync(x => x.ListGuid == ListGuid);
        }
        public async Task<IEnumerable<TodoLists>> GetTodoLists(PageParmeters pageParmeters)
        {
            IQueryable<TodoLists> query = appDbContext.TodoLists.Where(x => x.CreatedBy == LoginUser);

            var result = await query.Skip((pageParmeters.pageNumber - 1) * pageParmeters.pageSize).Take(pageParmeters.pageSize).ToListAsync();

            return result;

            //return await appDbContext.TodoLists.Skip((pageParmeters.pageNumber-1)*pageParmeters.pageSize).Take(pageParmeters.pageSize).ToListAsync();
        }

        public async Task<IEnumerable<TodoLists>> Search(string ListName)
        {

            IQueryable<TodoLists> query = appDbContext.TodoLists.Where(x => x.CreatedBy == LoginUser);

            if (!string.IsNullOrEmpty(ListName))
            {
                query = query.Where(x => x.TodoListName.Contains(ListName));
            }

            return await query.ToListAsync();
        }

        public async Task<TodoLists> UpdateTodoList(TodoLists TodoListitem)
        {
            var result = await appDbContext.TodoLists
                .FirstOrDefaultAsync(x => x.Id == TodoListitem.Id);

            if (result != null)
            {
                result.TodoListName = TodoListitem.TodoListName;

                result.ModifiedDateTime = DateTime.Now;

                await appDbContext.SaveChangesAsync();

                return result;
            }

            return null;
        }


    }
}
