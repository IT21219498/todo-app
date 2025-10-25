using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;

namespace TodoApp.Tests.Repositories;

/// <summary>
/// Unit tests for TaskRepository
/// Tests data access logic using in-memory database
/// </summary>
public class TaskRepositoryTests : IDisposable
{
    private readonly TodoDbContext _context;
    private readonly ITaskRepository _repository;

    public TaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TodoDbContext(options);
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public async Task GetRecentTasksAsync_ShouldReturnRecentIncompleteTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new() { Title = "Task 1", Description = "Desc 1", Completed = false, CreatedAt = DateTime.UtcNow.AddDays(-3) },
            new() { Title = "Task 2", Description = "Desc 2", Completed = false, CreatedAt = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Task 3", Description = "Desc 3", Completed = true, CreatedAt = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Task 4", Description = "Desc 4", Completed = false, CreatedAt = DateTime.UtcNow }
        };
        await _context.Tasks.AddRangeAsync(tasks);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRecentTasksAsync(5, includeCompleted: false);

        // Assert
        var resultList = result.ToList();
        Assert.Equal(3, resultList.Count); // Only incomplete tasks
        Assert.Equal("Task 4", resultList[0].Title); // Most recent first
        Assert.All(resultList, task => Assert.False(task.Completed));
    }

    [Fact]
    public async Task GetRecentTasksAsync_WithLimit_ShouldReturnLimitedResults()
    {
        // Arrange
        for (int i = 1; i <= 10; i++)
        {
            await _context.Tasks.AddAsync(new TaskEntity
            {
                Title = $"Task {i}",
                Description = $"Desc {i}",
                Completed = false,
                CreatedAt = DateTime.UtcNow.AddDays(-i)
            });
        }
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRecentTasksAsync(5, includeCompleted: false);

        // Assert
        Assert.Equal(5, result.Count());
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenExists_ShouldReturnTask()
    {
        // Arrange
        var task = new TaskEntity
        {
            Title = "Test Task",
            Description = "Test Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTaskByIdAsync(task.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetTaskByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldAddTaskToDatabase()
    {
        // Arrange
        var task = new TaskEntity
        {
            Title = "New Task",
            Description = "New Description"
        };

        // Act
        var result = await _repository.CreateTaskAsync(task);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.Equal("New Task", result.Title);
        Assert.False(result.Completed);
        Assert.NotEqual(default, result.CreatedAt);

        var savedTask = await _context.Tasks.FindAsync(result.Id);
        Assert.NotNull(savedTask);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask()
    {
        // Arrange
        var task = new TaskEntity
        {
            Title = "Original Title",
            Description = "Original Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        task.Title = "Updated Title";
        task.Completed = true;
        var result = await _repository.UpdateTaskAsync(task);

        // Assert
        Assert.Equal("Updated Title", result.Title);
        Assert.True(result.Completed);
        Assert.NotNull(result.UpdatedAt);

        var updatedTask = await _context.Tasks.FindAsync(task.Id);
        Assert.Equal("Updated Title", updatedTask!.Title);
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        var task = new TaskEntity
        {
            Title = "Task to Delete",
            Description = "Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteTaskAsync(task.Id);

        // Assert
        Assert.True(result);
        var deletedTask = await _context.Tasks.FindAsync(task.Id);
        Assert.Null(deletedTask);
    }

    [Fact]
    public async Task DeleteTaskAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.DeleteTaskAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task TaskExistsAsync_WhenExists_ShouldReturnTrue()
    {
        // Arrange
        var task = new TaskEntity
        {
            Title = "Existing Task",
            Description = "Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.TaskExistsAsync(task.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task TaskExistsAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.TaskExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
