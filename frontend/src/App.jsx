import { useState, useEffect } from "react";
import TaskForm from "./components/TaskForm";
import TaskList from "./components/TaskList";
import { getTasks, createTask, markTaskAsCompleted } from "./services/api";
import "./App.css";

/**
 * Main App component for the Todo Task application
 */
function App() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Fetch tasks on component mount
  useEffect(() => {
    fetchTasks();
  }, []);

  /**
   * Fetches tasks from the API
   */
  const fetchTasks = async () => {
    try {
      setLoading(true);
      setError("");
      const data = await getTasks();
      setTasks(data);
    } catch {
      setError("Failed to load tasks. Please check your connection.");
    } finally {
      setLoading(false);
    }
  };

  /**
   * Handles task creation
   * @param {Object} taskData - The task data (title, description)
   */
  const handleTaskCreate = async (taskData) => {
    try {
      await createTask(taskData);
      // Refresh the task list after creating a new task
      await fetchTasks();
    } catch {
      throw new Error("Failed to create task");
    }
  };

  /**
   * Handles marking a task as completed
   * @param {number} taskId - The ID of the task to complete
   */
  const handleTaskComplete = async (taskId) => {
    try {
      await markTaskAsCompleted(taskId);
      // Remove the completed task from the UI
      setTasks((prevTasks) => prevTasks.filter((task) => task.id !== taskId));
    } catch {
      setError("Failed to complete task. Please try again.");
    }
  };

  return (
    <div className='app'>
      <header className='app-header'>
        <h1>Todo Tasks</h1>
        <p className='subtitle'>Manage your daily tasks efficiently</p>
      </header>

      <main className='app-content'>
        <div className='content-wrapper'>
          <TaskForm onTaskCreate={handleTaskCreate} />

          {error && (
            <div className='error-banner'>
              {error}
              <button onClick={() => setError("")} className='close-error'>
                ×
              </button>
            </div>
          )}

          <TaskList
            tasks={tasks}
            onTaskComplete={handleTaskComplete}
            loading={loading}
          />
        </div>
      </main>
    </div>
  );
}

export default App;
