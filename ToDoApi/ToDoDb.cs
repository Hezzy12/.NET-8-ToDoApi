using Microsoft.EntityFrameworkCore;

namespace ToDoApi
{
    public class ToDoDb: DbContext
    {
        public ToDoDb(DbContextOptions<ToDoDb> options) : base(options)
        {

        }

        public DbSet<ToDo> Todos => Set<ToDo>();
    }
}
