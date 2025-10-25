using TodoApp.Api.DTOs;
using TodoApp.Api.Models;
using TodoApp.Api.Repositories;

namespace TodoApp.Api.Services;

/// <summary>
/// Service implementation for task business logic
/// Implements Single Responsibility Principle - handles business logic only
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskResponseDto>> GetRecentTasksAsync(int limit = 5)
    {
        var tasks = await _taskRepository.GetRecentTasksAsync(limit, includeCompleted: false);
        return tasks.Select(MapToResponseDto);
    }

    /// <inheritdoc />
    public async Task<TaskResponseDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        return task != null ? MapToResponseDto(task) : null;
    }

    /// <inheritdoc />
    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        if (createTaskDto == null)
            throw new ArgumentNullException(nameof(createTaskDto));

        // Validate business rules
        if (string.IsNullOrWhiteSpace(createTaskDto.Title))
            throw new ArgumentException("Title cannot be empty", nameof(createTaskDto.Title));

        var taskEntity = new TaskEntity
        {
            Title = createTaskDto.Title.Trim(),
            Description = createTaskDto.Description?.Trim() ?? string.Empty,
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await _taskRepository.CreateTaskAsync(taskEntity);
        return MapToResponseDto(createdTask);
    }

    /// <inheritdoc />
    public async Task<TaskResponseDto?> MarkTaskAsCompletedAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        
        if (task == null)
            return null;

        // Business logic: only mark incomplete tasks as completed
        if (task.Completed)
            return MapToResponseDto(task); // Already completed, return as is

        task.Completed = true;
        task.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskRepository.UpdateTaskAsync(task);
        return MapToResponseDto(updatedTask);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await _taskRepository.DeleteTaskAsync(id);
    }

    /// <summary>
    /// Maps TaskEntity to TaskResponseDto
    /// Follows Single Responsibility - handles mapping logic
    /// </summary>
    private static TaskResponseDto MapToResponseDto(TaskEntity task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Completed = task.Completed,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
