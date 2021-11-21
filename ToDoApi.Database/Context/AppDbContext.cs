using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoApi.Database.Models;

namespace ToDoApi.Database.Context
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext()
        {
               
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {

        }

        public DbSet<TodoLists> TodoLists { get; set; }
        public DbSet<TodoItems> TodoItems { get; set; }
        public DbSet<Labels> Labels { get; set; }
        public DbSet<AssignLabels> AssignLabels { get; set; }
    }
}
