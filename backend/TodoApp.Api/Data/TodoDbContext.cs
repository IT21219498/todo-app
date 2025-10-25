using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Models;

namespace TodoApp.Api.Data;

/// <summary>
/// Database context for the Todo application
/// </summary>
public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for Task entities
    /// </summary>
    public DbSet<TaskEntity> Tasks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Task entity
        modelBuilder.Entity<TaskEntity>(entity =>
        {
            entity.ToTable("task");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            entity.Property(e => e.Completed)
                .HasColumnName("completed")
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            // Create index on CreatedAt for efficient sorting
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("ix_task_created_at");

            // Create index on Completed for filtering
            entity.HasIndex(e => e.Completed)
                .HasDatabaseName("ix_task_completed");
        });
    }
}
