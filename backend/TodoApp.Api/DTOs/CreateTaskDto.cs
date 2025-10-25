using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.DTOs;

/// <summary>
/// Data Transfer Object for creating a new task
/// </summary>
public class CreateTaskDto
{
    /// <summary>
    /// Title of the task
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the task
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;
}
