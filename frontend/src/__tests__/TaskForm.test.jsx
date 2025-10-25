import { describe, it, expect, vi } from "vitest";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import TaskForm from "../components/TaskForm";

describe("TaskForm", () => {
  it("renders the form with title and description inputs", () => {
    const mockOnTaskCreate = vi.fn();
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    expect(screen.getByLabelText(/title/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/description/i)).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /add/i })).toBeInTheDocument();
  });

  it("updates input values when user types", () => {
    const mockOnTaskCreate = vi.fn();
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    const titleInput = screen.getByLabelText(/title/i);
    const descriptionInput = screen.getByLabelText(/description/i);

    fireEvent.change(titleInput, { target: { value: "Test Task" } });
    fireEvent.change(descriptionInput, {
      target: { value: "Test Description" },
    });

    expect(titleInput.value).toBe("Test Task");
    expect(descriptionInput.value).toBe("Test Description");
  });

  it("shows error message when submitting without title", async () => {
    const mockOnTaskCreate = vi.fn();
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    const submitButton = screen.getByRole("button", { name: /add/i });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText(/title is required/i)).toBeInTheDocument();
    });

    expect(mockOnTaskCreate).not.toHaveBeenCalled();
  });

  it("calls onTaskCreate with correct data when form is submitted", async () => {
    const mockOnTaskCreate = vi.fn().mockResolvedValue({});
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    const titleInput = screen.getByLabelText(/title/i);
    const descriptionInput = screen.getByLabelText(/description/i);
    const submitButton = screen.getByRole("button", { name: /add/i });

    fireEvent.change(titleInput, { target: { value: "Test Task" } });
    fireEvent.change(descriptionInput, {
      target: { value: "Test Description" },
    });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(mockOnTaskCreate).toHaveBeenCalledWith({
        title: "Test Task",
        description: "Test Description",
      });
    });
  });

  it("clears form fields after successful submission", async () => {
    const mockOnTaskCreate = vi.fn().mockResolvedValue({});
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    const titleInput = screen.getByLabelText(/title/i);
    const descriptionInput = screen.getByLabelText(/description/i);
    const submitButton = screen.getByRole("button", { name: /add/i });

    fireEvent.change(titleInput, { target: { value: "Test Task" } });
    fireEvent.change(descriptionInput, {
      target: { value: "Test Description" },
    });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(titleInput.value).toBe("");
      expect(descriptionInput.value).toBe("");
    });
  });

  it("shows error message when task creation fails", async () => {
    const mockOnTaskCreate = vi.fn().mockRejectedValue(new Error("API Error"));
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    const titleInput = screen.getByLabelText(/title/i);
    const submitButton = screen.getByRole("button", { name: /add/i });

    fireEvent.change(titleInput, { target: { value: "Test Task" } });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText(/failed to create task/i)).toBeInTheDocument();
    });
  });

  it("disables form inputs while submitting", async () => {
    const mockOnTaskCreate = vi
      .fn()
      .mockImplementation(
        () => new Promise((resolve) => setTimeout(resolve, 100))
      );
    render(<TaskForm onTaskCreate={mockOnTaskCreate} />);

    const titleInput = screen.getByLabelText(/title/i);
    const submitButton = screen.getByRole("button", { name: /add/i });

    fireEvent.change(titleInput, { target: { value: "Test Task" } });
    fireEvent.click(submitButton);

    expect(titleInput).toBeDisabled();
    expect(submitButton).toBeDisabled();
    expect(submitButton).toHaveTextContent(/adding/i);

    await waitFor(() => {
      expect(titleInput).not.toBeDisabled();
      expect(submitButton).not.toBeDisabled();
    });
  });
});
