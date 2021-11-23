﻿using Microsoft.AspNetCore.Http;
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
    public class TodoListsRepository : ITodoListsRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly Guid _loginUser;
        public TodoListsRepository(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            if (httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                IEnumerable<Claim> claims = identity.Claims;
                _loginUser = new Guid(claims.ToList()[0].Value);
            }
        }

        public async Task<TodoLists> AddTodoList(TodoLists todoList)
        {
            todoList.CreatedDateTime = DateTime.Now;
            todoList.ListGuid = Guid.NewGuid();
            todoList.CreatedBy = _loginUser;
            var result = await _appDbContext.TodoLists.AddAsync(todoList);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteTodoList(int Id)
        {
            var result = await _appDbContext.TodoLists
                .FirstOrDefaultAsync(x => x.Id == Id && x.CreatedBy == _loginUser);
            if (result != null)
            {
                var assignedresult = await _appDbContext.AssignLabels.Where(x => x.AssignedGuid == result.ListGuid).ToListAsync();
                foreach (var assignedlabel in assignedresult)
                {
                    _appDbContext.AssignLabels.Remove(assignedlabel);
                }
                _appDbContext.TodoLists.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<TodoLists> GetTodoList(int Id)
        {
            return await _appDbContext.TodoLists.Include(x => x.TodoItems)
                  .FirstOrDefaultAsync(y => y.Id == Id && y.CreatedBy == _loginUser);
        }

        public async Task<TodoLists> GetTodoListByName(string TodoListName)
        {
            return await _appDbContext.TodoLists.Include(x => x.TodoItems)
                .FirstOrDefaultAsync(x => x.TodoListName == TodoListName && x.CreatedBy == _loginUser);
        }

        public async Task<TodoLists> GetTodoListByGuid(Guid ItemGuid)
        {
            return await _appDbContext.TodoLists.Include(x => x.TodoItems)
                .FirstOrDefaultAsync(x => x.ListGuid == ItemGuid);
        }
        public async Task<IEnumerable<TodoLists>> GetTodoLists(PageParmeters pageParmeters)
        {
            IQueryable<TodoLists> query = _appDbContext.TodoLists.Include(x=>x.TodoItems).Where(x => x.CreatedBy == _loginUser);
            var result = await query.Skip((pageParmeters.PageNumber - 1) * pageParmeters.PageSize).Take(pageParmeters.PageSize).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<TodoLists>> Search(string TodoListName)
        {
            IQueryable<TodoLists> query = _appDbContext.TodoLists.Where(x => x.CreatedBy == _loginUser);
            if (!string.IsNullOrEmpty(TodoListName))
            {
                query = query.Where(x => x.TodoListName.Contains(TodoListName));
            }
            return await query.ToListAsync();
        }

        public async Task<TodoLists> UpdateTodoList(TodoLists todoList)
        {
            var result = await _appDbContext.TodoLists
                .FirstOrDefaultAsync(x => x.Id == todoList.Id);
            if (result != null)
            {
                result.TodoListName = todoList.TodoListName;
                result.ModifiedDateTime = DateTime.Now;
                await _appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
