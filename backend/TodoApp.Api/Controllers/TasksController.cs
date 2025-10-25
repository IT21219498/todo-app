using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.DTOs;
using TodoApp.Api.Services;

namespace TodoApp.Api.Controllers;

/// <summary>
/// Controller for managing tasks
/// Follows RESTful API conventions
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets the 5 most recent incomplete tasks
    /// </summary>
    /// <returns>List of tasks</returns>
    /// <response code="200">Returns the list of tasks</response>
    /// <response code="500">If an error occurred</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
    {
        try
        {
            _logger.LogInformation("Fetching recent tasks");
            var tasks = await _taskService.GetRecentTasksAsync(5);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tasks");
            return StatusCode(500, new { message = "An error occurred while fetching tasks" });
        }
    }

    /// <summary>
    /// Gets a specific task by ID
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>Task details</returns>
    /// <response code="200">Returns the task</response>
    /// <response code="404">If task is not found</response>
    /// <response code="500">If an error occurred</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
    {
        try
        {
            _logger.LogInformation("Fetching task with ID: {TaskId}", id);
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found", id);
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching task with ID: {TaskId}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the task" });
        }
    }

    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="createTaskDto">Task creation data</param>
    /// <returns>Created task</returns>
    /// <response code="201">Returns the newly created task</response>
    /// <response code="400">If the task data is invalid</response>
    /// <response code="500">If an error occurred</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskResponseDto>> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid task data received");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new task: {Title}", createTaskDto.Title);
            var createdTask = await _taskService.CreateTaskAsync(createTaskDto);

            return CreatedAtAction(
                nameof(GetTask),
                new { id = createdTask.Id },
                createdTask
            );
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument while creating task");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, new { message = "An error occurred while creating the task" });
        }
    }

    /// <summary>
    /// Marks a task as completed
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>Updated task</returns>
    /// <response code="200">Returns the updated task</response>
    /// <response code="404">If task is not found</response>
    /// <response code="500">If an error occurred</response>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TaskResponseDto>> CompleteTask(int id)
    {
        try
        {
            _logger.LogInformation("Marking task {TaskId} as completed", id);
            var updatedTask = await _taskService.MarkTaskAsCompletedAsync(id);

            if (updatedTask == null)
            {
                _logger.LogWarning("Task with ID {TaskId} not found", id);
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            return Ok(updatedTask);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing task with ID: {TaskId}", id);
            return StatusCode(500, new { message = "An error occurred while completing the task" });
        }
    }

    /// <summary>
    /// Deletes a task
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <returns>No content</returns>
    /// <response code="204">If task was deleted successfully</response>
    /// <response code="404">If task is not found</response>
    /// <response code="500">If an error occurred</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            _logger.LogInformation("Deleting task with ID: {TaskId}", id);
            var deleted = await _taskService.DeleteTaskAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Task with ID {TaskId} not found", id);
                return NotFound(new { message = $"Task with ID {id} not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task with ID: {TaskId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the task" });
        }
    }
}
