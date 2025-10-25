namespace TodoApp.Api.DTOs;

/// <summary>
/// Data Transfer Object for task response
/// </summary>
public class TaskResponseDto
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the task
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the task
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the task has been completed
    /// </summary>
    public bool Completed { get; set; }

    /// <summary>
    /// Timestamp when the task was created (UTC)
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the task was last updated (UTC)
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
