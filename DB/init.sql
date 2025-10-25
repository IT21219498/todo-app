-- Initialize the database schema for the Todo application (MySQL)

-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS tododb;
USE tododb;

-- Create task table
CREATE TABLE IF NOT EXISTS task (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    completed BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT chk_title_not_empty CHECK (CHAR_LENGTH(TRIM(title)) > 0)
);

-- Create indexes for performance
CREATE INDEX ix_task_created_at ON task(created_at DESC);
CREATE INDEX ix_task_completed ON task(completed);

-- Insert sample data (optional, for testing)
INSERT INTO task (title, description, completed, created_at) VALUES
('Buy books', 'Buy books for the next school year', FALSE, CURRENT_TIMESTAMP),
('Clean home', 'Need to clean the bed room', FALSE, DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 1 DAY)),
('Takehome assignment', 'Finish the mid-term assignment', FALSE, DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 2 DAY)),
('Play Cricket', 'Plan the soft ball cricket match on next Sunday', FALSE, DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 3 DAY)),
('Help Saman', 'Saman need help with his software project', FALSE, DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 4 DAY));

