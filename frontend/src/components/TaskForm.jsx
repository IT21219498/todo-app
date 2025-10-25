import { useState } from "react";
import PropTypes from "prop-types";
import "./TaskForm.css";

/**
 * TaskForm component for creating new tasks
 * @param {Object} props
 * @param {Function} props.onTaskCreate - Callback function called when a task is created
 */
const TaskForm = ({ onTaskCreate }) => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validation
    if (!title.trim()) {
      setError("Title is required");
      return;
    }

    setIsSubmitting(true);
    setError("");

    try {
      await onTaskCreate({ title, description });
      // Reset form on success
      setTitle("");
      setDescription("");
    } catch {
      setError("Failed to create task. Please try again.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className='task-form-container'>
      <h2>Add a Task</h2>
      <form onSubmit={handleSubmit} className='task-form'>
        <div className='form-group'>
          <label htmlFor='title'>Title</label>
          <input
            id='title'
            type='text'
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder='Enter task title'
            disabled={isSubmitting}
            maxLength={100}
          />
        </div>

        <div className='form-group'>
          <label htmlFor='description'>Description</label>
          <textarea
            id='description'
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            placeholder='Enter task description'
            rows='4'
            disabled={isSubmitting}
            maxLength={500}
          />
        </div>

        {error && <div className='error-message'>{error}</div>}

        <button type='submit' className='add-button' disabled={isSubmitting}>
          {isSubmitting ? "Adding..." : "Add"}
        </button>
      </form>
    </div>
  );
};

TaskForm.propTypes = {
  onTaskCreate: PropTypes.func.isRequired,
};

export default TaskForm;
