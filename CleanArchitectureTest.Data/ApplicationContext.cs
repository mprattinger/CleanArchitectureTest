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

        modelBuilder.Entity<TodoAppointee>()
            .HasOne(ta => ta.Todo)
            .WithMany(t => t.TodoAppointees)
            .HasForeignKey(ta => ta.AppointeeId);

        modelBuilder.Entity<TodoAppointee>()
            .HasOne(ta => ta.Appointee)
            .WithMany(m => m.TodoAppointees)
            .HasForeignKey(ta => ta.TodoId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=todo.db");
        }
    }
}
