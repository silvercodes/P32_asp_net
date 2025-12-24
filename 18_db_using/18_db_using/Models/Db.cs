using Microsoft.EntityFrameworkCore;

namespace _18_db_using.Models;

public class Db: DbContext
{
    public Db(DbContextOptions<Db> options)
        : base(options)
    { }
    public DbSet<TaskItem> TaskItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>()
            .Property(t => t.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<TaskItem>()
            .HasIndex(t => t.Title);

        modelBuilder.Entity<TaskItem>()
            .Property(t => t.IsCompleted)
            .HasDefaultValue(false);

        // .....
    }
}
