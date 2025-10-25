namespace TodoApp.Api.Models;

/// <summary>
/// Represents a task entity in the system
/// </summary>
public class TaskEntity
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Title of the task (required, max 100 characters)
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the task (max 500 characters)
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the task has been completed
    /// </summary>
    public bool Completed { get; set; } = false;

    /// <summary>
    /// Timestamp when the task was created (UTC)
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the task was last updated (UTC)
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
