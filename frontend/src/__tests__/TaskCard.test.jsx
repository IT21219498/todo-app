import { describe, it, expect, vi } from "vitest";
import { render, screen, fireEvent } from "@testing-library/react";
import TaskCard from "../components/TaskCard";

describe("TaskCard", () => {
  const mockTask = {
    id: 1,
    title: "Test Task",
    description: "Test Description",
  };

  it("renders task title and description", () => {
    const mockOnTaskComplete = vi.fn();
    render(<TaskCard task={mockTask} onTaskComplete={mockOnTaskComplete} />);

    expect(screen.getByText("Test Task")).toBeInTheDocument();
    expect(screen.getByText("Test Description")).toBeInTheDocument();
  });

  it("renders the Done button", () => {
    const mockOnTaskComplete = vi.fn();
    render(<TaskCard task={mockTask} onTaskComplete={mockOnTaskComplete} />);

    const doneButton = screen.getByRole("button", { name: /done/i });
    expect(doneButton).toBeInTheDocument();
  });

  it("calls onTaskComplete with task id when Done button is clicked", () => {
    const mockOnTaskComplete = vi.fn();
    render(<TaskCard task={mockTask} onTaskComplete={mockOnTaskComplete} />);

    const doneButton = screen.getByRole("button", { name: /done/i });
    fireEvent.click(doneButton);

    expect(mockOnTaskComplete).toHaveBeenCalledWith(1);
    expect(mockOnTaskComplete).toHaveBeenCalledTimes(1);
  });

  it("renders long title and description correctly", () => {
    const longTask = {
      id: 2,
      title: "This is a very long title that should wrap correctly in the UI",
      description:
        "This is a very long description that contains multiple sentences. It should display properly and wrap to multiple lines if necessary.",
    };
    const mockOnTaskComplete = vi.fn();
    render(<TaskCard task={longTask} onTaskComplete={mockOnTaskComplete} />);

    expect(screen.getByText(longTask.title)).toBeInTheDocument();
    expect(screen.getByText(longTask.description)).toBeInTheDocument();
  });

  it("has proper accessibility label for Done button", () => {
    const mockOnTaskComplete = vi.fn();
    render(<TaskCard task={mockTask} onTaskComplete={mockOnTaskComplete} />);

    const doneButton = screen.getByLabelText('Mark "Test Task" as done');
    expect(doneButton).toBeInTheDocument();
  });
});
