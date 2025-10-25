import PropTypes from "prop-types";
import "./TaskCard.css";

/**
 * TaskCard component displays a single task
 * @param {Object} props
 * @param {Object} props.task - Task object
 * @param {number} props.task.id - Task ID
 * @param {string} props.task.title - Task title
 * @param {string} props.task.description - Task description
 * @param {Function} props.onTaskComplete - Callback function when task is marked as complete
 */
const TaskCard = ({ task, onTaskComplete }) => {
  const handleDoneClick = () => {
    onTaskComplete(task.id);
  };

  return (
    <div className='task-card'>
      <div className='task-card-content'>
        <h3 className='task-title'>{task.title}</h3>
        <p className='task-description'>{task.description}</p>
      </div>
      <button
        className='done-button'
        onClick={handleDoneClick}
        aria-label={`Mark "${task.title}" as done`}
      >
        Done
      </button>
    </div>
  );
};

TaskCard.propTypes = {
  task: PropTypes.shape({
    id: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
  }).isRequired,
  onTaskComplete: PropTypes.func.isRequired,
};

export default TaskCard;
