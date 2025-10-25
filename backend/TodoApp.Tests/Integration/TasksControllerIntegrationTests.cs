using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TodoApp.Api.Data;

namespace TodoApp.Tests.Integration;

/// <summary>
/// Integration tests for TasksController
/// Tests end-to-end API functionality
/// </summary>
public class TasksControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public TasksControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TodoDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add in-memory database for testing
                services.AddDbContext<TodoDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid());
                });

                // Build service provider and create database
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
                db.Database.EnsureCreated();
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var tasks = await response.Content.ReadFromJsonAsync<List<TaskResponseDto>>();
        Assert.NotNull(tasks);
    }

    [Fact]
    public async Task CreateTask_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var createDto = new CreateTaskDto
        {
            Title = "Integration Test Task",
            Description = "Integration Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdTask = await response.Content.ReadFromJsonAsync<TaskResponseDto>();
        Assert.NotNull(createdTask);
        Assert.Equal("Integration Test Task", createdTask.Title);
        Assert.False(createdTask.Completed);

        // Verify Location header
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task CreateTask_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange - title is required
        var createDto = new CreateTaskDto
        {
            Title = "",
            Description = "Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTaskById_WhenExists_ShouldReturnTask()
    {
        // Arrange - Create a task first
        var createDto = new CreateTaskDto
        {
            Title = "Test Task for Get",
            Description = "Description"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/tasks", createDto);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponseDto>();

        // Act
        var response = await _client.GetAsync($"/api/tasks/{createdTask!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var task = await response.Content.ReadFromJsonAsync<TaskResponseDto>();
        Assert.NotNull(task);
        Assert.Equal(createdTask.Id, task.Id);
    }

    [Fact]
    public async Task GetTaskById_WhenNotExists_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/tasks/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CompleteTask_WhenExists_ShouldMarkAsCompleted()
    {
        // Arrange - Create a task first
        var createDto = new CreateTaskDto
        {
            Title = "Task to Complete",
            Description = "Description"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/tasks", createDto);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponseDto>();

        // Act
        var response = await _client.PatchAsync($"/api/tasks/{createdTask!.Id}/complete", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var completedTask = await response.Content.ReadFromJsonAsync<TaskResponseDto>();
        Assert.NotNull(completedTask);
        Assert.True(completedTask.Completed);
        Assert.NotNull(completedTask.UpdatedAt);
    }

    [Fact]
    public async Task CompleteTask_WhenNotExists_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.PatchAsync("/api/tasks/99999/complete", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WhenExists_ShouldReturnNoContent()
    {
        // Arrange - Create a task first
        var createDto = new CreateTaskDto
        {
            Title = "Task to Delete",
            Description = "Description"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/tasks", createDto);
        var createdTask = await createResponse.Content.ReadFromJsonAsync<TaskResponseDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/tasks/{createdTask!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify task is deleted
        var getResponse = await _client.GetAsync($"/api/tasks/{createdTask.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WhenNotExists_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/tasks/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTasks_ShouldReturnOnlyIncompleteTasks()
    {
        // Arrange - Create multiple tasks and complete one
        var task1 = await _client.PostAsJsonAsync("/api/tasks", new CreateTaskDto { Title = "Task 1", Description = "Desc 1" });
        var task2 = await _client.PostAsJsonAsync("/api/tasks", new CreateTaskDto { Title = "Task 2", Description = "Desc 2" });
        var task3 = await _client.PostAsJsonAsync("/api/tasks", new CreateTaskDto { Title = "Task 3", Description = "Desc 3" });

        var task2Data = await task2.Content.ReadFromJsonAsync<TaskResponseDto>();
        await _client.PatchAsync($"/api/tasks/{task2Data!.Id}/complete", null);

        // Act
        var response = await _client.GetAsync("/api/tasks");

        // Assert
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskResponseDto>>();
        Assert.NotNull(tasks);
        Assert.DoesNotContain(tasks, t => t.Completed);
        Assert.Contains(tasks, t => t.Title == "Task 1");
        Assert.Contains(tasks, t => t.Title == "Task 3");
        Assert.DoesNotContain(tasks, t => t.Title == "Task 2");
    }

    [Fact]
    public async Task HealthCheck_ShouldReturnHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("healthy", content);
    }
}
