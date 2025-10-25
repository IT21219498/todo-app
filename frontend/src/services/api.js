/**
 * API service for communicating with the backend
 */

const API_BASE_URL =
  import.meta.env.VITE_API_URL || "http://localhost:3000/api";

/**
 * Fetches the most recent 5 tasks from the backend
 * @returns {Promise<Array>} Array of task objects
 */
export const getTasks = async () => {
  try {
    const response = await fetch(`${API_BASE_URL}/tasks`);
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.json();
  } catch (error) {
    console.error("Error fetching tasks:", error);
    throw error;
  }
};

/**
 * Creates a new task
 * @param {Object} taskData - The task data
 * @param {string} taskData.title - Task title
 * @param {string} taskData.description - Task description
 * @returns {Promise<Object>} Created task object
 */
export const createTask = async (taskData) => {
  try {
    const response = await fetch(`${API_BASE_URL}/tasks`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(taskData),
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.json();
  } catch (error) {
    console.error("Error creating task:", error);
    throw error;
  }
};

/**
 * Marks a task as completed
 * @param {number} taskId - The ID of the task to mark as completed
 * @returns {Promise<Object>} Updated task object
 */
export const markTaskAsCompleted = async (taskId) => {
  try {
    const response = await fetch(`${API_BASE_URL}/tasks/${taskId}/complete`, {
      method: "PATCH",
      headers: {
        "Content-Type": "application/json",
      },
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    return await response.json();
  } catch (error) {
    console.error("Error marking task as completed:", error);
    throw error;
  }
};
