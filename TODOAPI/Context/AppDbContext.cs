using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Context
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {

        }

        public DbSet<TodoLists> TodoLists { get; set; }
        public DbSet<TodoItems> TodoItems { get; set; }
        public DbSet<Labels> Labels { get; set; }
        public DbSet<AssignLabels> AssignLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed TodoList Table
            //modelBuilder.Entity<TodoLists>().HasData(
            //    new TodoLists { Id = 1,TodoListName = "Test" ,CreatedDateTime=DateTime.Now,ListGuid=new Guid()});

            ////Seed TodoItems Table
            //modelBuilder.Entity<TodoItems>().HasData(
            //    new TodoItems { ItemID = 1, ItemName = "TestItem",ListId=1,CreatedDateTime=DateTime.Now,ItemGuid=new Guid() });

        }
    }
}
