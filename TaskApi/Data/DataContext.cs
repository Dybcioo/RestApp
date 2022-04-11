using Microsoft.EntityFrameworkCore;
using TaskApi.Entities;

namespace TaskApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DataContext() { }

        public virtual DbSet<TaskEntity> Tasks { get; set; }

    }
}
