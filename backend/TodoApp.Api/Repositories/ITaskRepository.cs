using TodoApp.Api.Models;

namespace TodoApp.Api.Repositories;

/// <summary>
/// Interface for Task repository operations
/// Following Repository Pattern and Dependency Inversion Principle
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Retrieves the most recent tasks
    /// </summary>
    /// <param name="limit">Maximum number of tasks to retrieve</param>
    /// <param name="includeCompleted">Whether to include completed tasks</param>
    /// <returns>List of tasks ordered by creation date (newest first)</returns>
    Task<IEnumerable<TaskEntity>> GetRecentTasksAsync(int limit = 5, bool includeCompleted = false);

    /// <summary>
    /// Retrieves a task by its ID
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>Task entity or null if not found</returns>
    Task<TaskEntity?> GetTaskByIdAsync(int id);

    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="task">Task entity to create</param>
    /// <returns>Created task with generated ID</returns>
    Task<TaskEntity> CreateTaskAsync(TaskEntity task);

    /// <summary>
    /// Updates an existing task
    /// </summary>
    /// <param name="task">Task entity with updated values</param>
    /// <returns>Updated task entity</returns>
    Task<TaskEntity> UpdateTaskAsync(TaskEntity task);

    /// <summary>
    /// Deletes a task by ID
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteTaskAsync(int id);

    /// <summary>
    /// Checks if a task exists
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> TaskExistsAsync(int id);
}
