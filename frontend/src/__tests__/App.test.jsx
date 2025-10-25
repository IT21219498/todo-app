import { describe, it, expect, vi, beforeEach } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import App from "../App";
import * as api from "../services/api";

// Mock the API module
vi.mock("../services/api");

describe("App", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("renders the app header", () => {
    api.getTasks.mockResolvedValue([]);
    render(<App />);

    expect(screen.getByText("Todo Tasks")).toBeInTheDocument();
    expect(screen.getByText(/manage your daily tasks/i)).toBeInTheDocument();
  });

  it("fetches and displays tasks on mount", async () => {
    const mockTasks = [
      { id: 1, title: "Task 1", description: "Description 1" },
      { id: 2, title: "Task 2", description: "Description 2" },
    ];

    api.getTasks.mockResolvedValue(mockTasks);
    render(<App />);

    await waitFor(() => {
      expect(screen.getByText("Task 1")).toBeInTheDocument();
      expect(screen.getByText("Task 2")).toBeInTheDocument();
    });
  });

  it("shows loading state initially", () => {
    api.getTasks.mockImplementation(() => new Promise(() => {})); // Never resolves
    render(<App />);

    expect(screen.getByText(/loading tasks/i)).toBeInTheDocument();
  });

  it("shows error message when fetching tasks fails", async () => {
    api.getTasks.mockRejectedValue(new Error("Network error"));
    render(<App />);

    await waitFor(() => {
      expect(screen.getByText(/failed to load tasks/i)).toBeInTheDocument();
    });
  });

  it("shows empty state when there are no tasks", async () => {
    api.getTasks.mockResolvedValue([]);
    render(<App />);

    await waitFor(() => {
      expect(screen.getByText(/no tasks yet/i)).toBeInTheDocument();
    });
  });

  it("renders the task form", async () => {
    api.getTasks.mockResolvedValue([]);
    render(<App />);

    await waitFor(() => {
      expect(screen.getByLabelText(/title/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/description/i)).toBeInTheDocument();
    });
  });
});
