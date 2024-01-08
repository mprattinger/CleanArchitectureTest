using CleanArchitectureTest.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTest.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Member> Members => Set<Member>();

    public DbSet<Todo> Todos => Set<Todo>();

    public DbSet<TodoAppointee> TodoAppointees => Set<TodoAppointee>();

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoAppointee>()
        .HasKey(ta => new { ta.TodoId, ta.AppointeeId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=todo.db");
        }
    }
}
