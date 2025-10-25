import { describe, it, expect, beforeEach, afterEach, vi } from "vitest";
import { getTasks, createTask, markTaskAsCompleted } from "../services/api";

// Mock fetch globally
globalThis.fetch = vi.fn();

describe("API Service", () => {
  beforeEach(() => {
    // Clear all mocks before each test
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  describe("getTasks", () => {
    it("fetches tasks successfully", async () => {
      const mockTasks = [
        { id: 1, title: "Task 1", description: "Description 1" },
        { id: 2, title: "Task 2", description: "Description 2" },
      ];

      globalThis.fetch.mockResolvedValueOnce({
        ok: true,
        json: async () => mockTasks,
      });

      const result = await getTasks();

      expect(globalThis.fetch).toHaveBeenCalledWith(
        "http://localhost:3000/api/tasks"
      );
      expect(result).toEqual(mockTasks);
    });

    it("throws error when fetch fails", async () => {
      globalThis.fetch.mockResolvedValueOnce({
        ok: false,
        status: 500,
      });

      await expect(getTasks()).rejects.toThrow("HTTP error! status: 500");
    });

    it("handles network errors", async () => {
      globalThis.fetch.mockRejectedValueOnce(new Error("Network error"));

      await expect(getTasks()).rejects.toThrow("Network error");
    });
  });

  describe("createTask", () => {
    it("creates a task successfully", async () => {
      const taskData = { title: "New Task", description: "New Description" };
      const mockResponse = { id: 1, ...taskData };

      globalThis.fetch.mockResolvedValueOnce({
        ok: true,
        json: async () => mockResponse,
      });

      const result = await createTask(taskData);

      expect(globalThis.fetch).toHaveBeenCalledWith(
        "http://localhost:3000/api/tasks",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(taskData),
        }
      );
      expect(result).toEqual(mockResponse);
    });

    it("throws error when creating task fails", async () => {
      const taskData = { title: "New Task", description: "New Description" };

      globalThis.fetch.mockResolvedValueOnce({
        ok: false,
        status: 400,
      });

      await expect(createTask(taskData)).rejects.toThrow(
        "HTTP error! status: 400"
      );
    });
  });

  describe("markTaskAsCompleted", () => {
    it("marks a task as completed successfully", async () => {
      const taskId = 1;
      const mockResponse = { id: taskId, completed: true };

      globalThis.fetch.mockResolvedValueOnce({
        ok: true,
        json: async () => mockResponse,
      });

      const result = await markTaskAsCompleted(taskId);

      expect(globalThis.fetch).toHaveBeenCalledWith(
        "http://localhost:3000/api/tasks/1/complete",
        {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );
      expect(result).toEqual(mockResponse);
    });

    it("throws error when marking task as completed fails", async () => {
      const taskId = 1;

      globalThis.fetch.mockResolvedValueOnce({
        ok: false,
        status: 404,
      });

      await expect(markTaskAsCompleted(taskId)).rejects.toThrow(
        "HTTP error! status: 404"
      );
    });
  });
});
