namespace TodoApp.Tests.Services;

/// <summary>
/// Unit tests for TaskService
/// Tests business logic following clean code principles
/// </summary>
public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepository;
    private readonly ITaskService _taskService;

    public TaskServiceTests()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _taskService = new TaskService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetRecentTasksAsync_ShouldReturnMappedTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new() { Id = 1, Title = "Task 1", Description = "Desc 1", Completed = false, CreatedAt = DateTime.UtcNow },
            new() { Id = 2, Title = "Task 2", Description = "Desc 2", Completed = false, CreatedAt = DateTime.UtcNow }
        };
        _mockRepository.Setup(r => r.GetRecentTasksAsync(5, false)).ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetRecentTasksAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Task 1", result.First().Title);
        _mockRepository.Verify(r => r.GetRecentTasksAsync(5, false), Times.Once);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskExists_ShouldReturnTask()
    {
        // Arrange
        var task = new TaskEntity 
        { 
            Id = 1, 
            Title = "Test Task", 
            Description = "Test Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        _mockRepository.Setup(r => r.GetTaskByIdAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTaskByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_WhenTaskDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetTaskByIdAsync(999)).ReturnsAsync((TaskEntity?)null);

        // Act
        var result = await _taskService.GetTaskByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTaskAsync_WithValidData_ShouldCreateTask()
    {
        // Arrange
        var createDto = new CreateTaskDto { Title = "New Task", Description = "New Description" };
        var createdTask = new TaskEntity 
        { 
            Id = 1, 
            Title = "New Task", 
            Description = "New Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        _mockRepository.Setup(r => r.CreateTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(createdTask);

        // Act
        var result = await _taskService.CreateTaskAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Task", result.Title);
        Assert.Equal("New Description", result.Description);
        Assert.False(result.Completed);
        _mockRepository.Verify(r => r.CreateTaskAsync(It.IsAny<TaskEntity>()), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_WithNullDto_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _taskService.CreateTaskAsync(null!));
    }

    [Fact]
    public async Task CreateTaskAsync_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var createDto = new CreateTaskDto { Title = "", Description = "Description" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _taskService.CreateTaskAsync(createDto));
    }

    [Fact]
    public async Task MarkTaskAsCompletedAsync_WhenTaskExists_ShouldMarkAsCompleted()
    {
        // Arrange
        var task = new TaskEntity 
        { 
            Id = 1, 
            Title = "Task", 
            Description = "Description",
            Completed = false,
            CreatedAt = DateTime.UtcNow
        };
        var completedTask = new TaskEntity 
        { 
            Id = 1, 
            Title = "Task", 
            Description = "Description",
            Completed = true,
            CreatedAt = task.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetTaskByIdAsync(1)).ReturnsAsync(task);
        _mockRepository.Setup(r => r.UpdateTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(completedTask);

        // Act
        var result = await _taskService.MarkTaskAsCompletedAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Completed);
        Assert.NotNull(result.UpdatedAt);
        _mockRepository.Verify(r => r.UpdateTaskAsync(It.IsAny<TaskEntity>()), Times.Once);
    }

    [Fact]
    public async Task MarkTaskAsCompletedAsync_WhenTaskDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetTaskByIdAsync(999)).ReturnsAsync((TaskEntity?)null);

        // Act
        var result = await _taskService.MarkTaskAsCompletedAsync(999);

        // Assert
        Assert.Null(result);
        _mockRepository.Verify(r => r.UpdateTaskAsync(It.IsAny<TaskEntity>()), Times.Never);
    }

    [Fact]
    public async Task MarkTaskAsCompletedAsync_WhenTaskAlreadyCompleted_ShouldReturnTaskWithoutUpdate()
    {
        // Arrange
        var task = new TaskEntity 
        { 
            Id = 1, 
            Title = "Task", 
            Description = "Description",
            Completed = true,
            CreatedAt = DateTime.UtcNow
        };
        _mockRepository.Setup(r => r.GetTaskByIdAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _taskService.MarkTaskAsCompletedAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Completed);
        _mockRepository.Verify(r => r.UpdateTaskAsync(It.IsAny<TaskEntity>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldCallRepository()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteTaskAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _taskService.DeleteTaskAsync(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteTaskAsync(1), Times.Once);
    }
}
