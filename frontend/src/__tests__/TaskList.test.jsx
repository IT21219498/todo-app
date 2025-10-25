import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import TaskList from "../components/TaskList";

describe("TaskList", () => {
  const mockTasks = [
    { id: 1, title: "Task 1", description: "Description 1" },
    { id: 2, title: "Task 2", description: "Description 2" },
    { id: 3, title: "Task 3", description: "Description 3" },
  ];

  it("renders loading message when loading is true", () => {
    const mockOnTaskComplete = vi.fn();
    render(
      <TaskList tasks={[]} onTaskComplete={mockOnTaskComplete} loading={true} />
    );

    expect(screen.getByText(/loading tasks/i)).toBeInTheDocument();
  });

  it("renders empty message when there are no tasks", () => {
    const mockOnTaskComplete = vi.fn();
    render(
      <TaskList
        tasks={[]}
        onTaskComplete={mockOnTaskComplete}
        loading={false}
      />
    );

    expect(screen.getByText(/no tasks yet/i)).toBeInTheDocument();
  });

  it("renders all tasks when tasks are provided", () => {
    const mockOnTaskComplete = vi.fn();
    render(
      <TaskList
        tasks={mockTasks}
        onTaskComplete={mockOnTaskComplete}
        loading={false}
      />
    );

    expect(screen.getByText("Task 1")).toBeInTheDocument();
    expect(screen.getByText("Task 2")).toBeInTheDocument();
    expect(screen.getByText("Task 3")).toBeInTheDocument();
  });

  it("renders correct number of task cards", () => {
    const mockOnTaskComplete = vi.fn();
    render(
      <TaskList
        tasks={mockTasks}
        onTaskComplete={mockOnTaskComplete}
        loading={false}
      />
    );

    const doneButtons = screen.getAllByRole("button", { name: /done/i });
    expect(doneButtons).toHaveLength(3);
  });

  it("passes onTaskComplete prop to each TaskCard", () => {
    const mockOnTaskComplete = vi.fn();
    const { container } = render(
      <TaskList
        tasks={mockTasks}
        onTaskComplete={mockOnTaskComplete}
        loading={false}
      />
    );

    const taskCards = container.querySelectorAll(".task-card");
    expect(taskCards).toHaveLength(3);
  });

  it("renders tasks in the order provided", () => {
    const mockOnTaskComplete = vi.fn();
    render(
      <TaskList
        tasks={mockTasks}
        onTaskComplete={mockOnTaskComplete}
        loading={false}
      />
    );

    const titles = screen.getAllByRole("heading", { level: 3 });
    expect(titles[0]).toHaveTextContent("Task 1");
    expect(titles[1]).toHaveTextContent("Task 2");
    expect(titles[2]).toHaveTextContent("Task 3");
  });
});
