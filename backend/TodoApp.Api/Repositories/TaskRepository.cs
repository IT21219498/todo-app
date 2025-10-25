using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.Models;

namespace TodoApp.Api.Repositories;

/// <summary>
/// Repository implementation for Task entity operations
/// Implements Repository Pattern for data access abstraction
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly TodoDbContext _context;

    public TaskRepository(TodoDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskEntity>> GetRecentTasksAsync(int limit = 5, bool includeCompleted = false)
    {
        var query = _context.Tasks.AsQueryable();

        if (!includeCompleted)
        {
            query = query.Where(t => !t.Completed);
        }

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<TaskEntity?> GetTaskByIdAsync(int id)
    {
        return await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <inheritdoc />
    public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        task.CreatedAt = DateTime.UtcNow;
        task.Completed = false;

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return task;
    }

    /// <inheritdoc />
    public async Task<TaskEntity> UpdateTaskAsync(TaskEntity task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        task.UpdatedAt = DateTime.UtcNow;

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();

        return task;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        
        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <inheritdoc />
    public async Task<bool> TaskExistsAsync(int id)
    {
        return await _context.Tasks.AnyAsync(t => t.Id == id);
    }
}
