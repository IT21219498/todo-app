using TodoApp.Api.DTOs;

namespace TodoApp.Api.Services;

/// <summary>
/// Interface for Task service operations
/// Following Interface Segregation and Dependency Inversion Principles
/// </summary>
public interface ITaskService
{
    /// <summary>
    /// Gets the most recent incomplete tasks
    /// </summary>
    /// <param name="limit">Maximum number of tasks to retrieve (default: 5)</param>
    /// <returns>List of task response DTOs</returns>
    Task<IEnumerable<TaskResponseDto>> GetRecentTasksAsync(int limit = 5);

    /// <summary>
    /// Gets a task by ID
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>Task response DTO or null if not found</returns>
    Task<TaskResponseDto?> GetTaskByIdAsync(int id);

    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="createTaskDto">Task creation data</param>
    /// <returns>Created task response DTO</returns>
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto);

    /// <summary>
    /// Marks a task as completed
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>Updated task response DTO or null if not found</returns>
    Task<TaskResponseDto?> MarkTaskAsCompletedAsync(int id);

    /// <summary>
    /// Deletes a task
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteTaskAsync(int id);
}
