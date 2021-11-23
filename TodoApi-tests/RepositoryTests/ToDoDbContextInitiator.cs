using Microsoft.EntityFrameworkCore;
using ToDoApi.Database.Context;

namespace TodoApi_tests.RepositoryTests
{
    public class ToDoDbContextInitiator
    {
        public AppDbContext DBContext { get; }
        public ToDoDbContextInitiator()
        {
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=TodoDB;Trusted_Connection=true");

            AppDbContext _toDoDbContext = new AppDbContext(builder.Options);
            DBContext = _toDoDbContext;
        }
    }
}