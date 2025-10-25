import PropTypes from "prop-types";
import TaskCard from "./TaskCard";
import "./TaskList.css";

/**
 * TaskList component displays a list of tasks
 * @param {Object} props
 * @param {Array} props.tasks - Array of task objects
 * @param {Function} props.onTaskComplete - Callback function when a task is completed
 * @param {boolean} props.loading - Loading state
 */
const TaskList = ({ tasks, onTaskComplete, loading }) => {
  if (loading) {
    return (
      <div className='task-list-container'>
        <div className='loading-message'>Loading tasks...</div>
      </div>
    );
  }

  if (tasks.length === 0) {
    return (
      <div className='task-list-container'>
        <div className='empty-message'>
          <p>No tasks yet. Create your first task!</p>
        </div>
      </div>
    );
  }

  return (
    <div className='task-list-container'>
      <div className='task-list'>
        {tasks.map((task) => (
          <TaskCard key={task.id} task={task} onTaskComplete={onTaskComplete} />
        ))}
      </div>
    </div>
  );
};

TaskList.propTypes = {
  tasks: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      title: PropTypes.string.isRequired,
      description: PropTypes.string.isRequired,
    })
  ).isRequired,
  onTaskComplete: PropTypes.func.isRequired,
  loading: PropTypes.bool,
};

TaskList.defaultProps = {
  loading: false,
};

export default TaskList;
