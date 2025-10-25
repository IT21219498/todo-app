# Todo App - Frontend

A modern, responsive React-based Single Page Application (SPA) for managing todo tasks. Built with React, Vite, and styled with custom CSS.

## Features

- ✅ Create new tasks with title and description
- ✅ View the 5 most recent tasks
- ✅ Mark tasks as completed (removes from UI)
- ✅ Responsive design with dark mode support
- ✅ Clean and intuitive user interface
- ✅ Comprehensive unit test coverage
- ✅ Dockerized for easy deployment

## Tech Stack

- **React 19** - UI library
- **Vite** - Build tool and dev server
- **PropTypes** - Runtime type checking
- **Vitest** - Unit testing framework
- **Testing Library** - React component testing
- **Nginx** - Production web server

## Project Structure

```
frontend/
├── public/              # Static assets
├── src/
│   ├── components/      # React components
│   │   ├── TaskCard.jsx
│   │   ├── TaskCard.css
│   │   ├── TaskForm.jsx
│   │   ├── TaskForm.css
│   │   ├── TaskList.jsx
│   │   └── TaskList.css
│   ├── services/        # API service layer
│   │   └── api.js
│   ├── __tests__/       # Unit tests
│   │   ├── App.test.jsx
│   │   ├── TaskCard.test.jsx
│   │   ├── TaskForm.test.jsx
│   │   ├── TaskList.test.jsx
│   │   └── api.test.js
│   ├── App.jsx          # Main application component
│   ├── App.css          # Application styles
│   ├── main.jsx         # Application entry point
│   ├── index.css        # Global styles
│   └── setupTests.js    # Test configuration
├── Dockerfile           # Docker configuration
├── nginx.conf           # Nginx configuration
├── vitest.config.js     # Vitest configuration
├── vite.config.js       # Vite configuration
└── package.json         # Dependencies and scripts
```

## Prerequisites

- Node.js 18+ and npm (for local development)
- Docker (for containerized deployment)

## Getting Started

### Local Development

1. **Install dependencies:**

   ```bash
   npm install
   ```

2. **Configure environment variables:**

   ```bash
   cp .env.example .env
   ```

   Edit `.env` to set the backend API URL:

   ```
   VITE_API_URL=http://localhost:3000/api
   ```

3. **Start development server:**

   ```bash
   npm run dev
   ```

   The application will be available at `http://localhost:5173`

4. **Run tests:**

   ```bash
   # Run tests once
   npm test

   # Run tests in watch mode
   npm run test:ui

   # Generate coverage report
   npm run test:coverage
   ```

5. **Build for production:**

   ```bash
   npm run build
   ```

   Preview production build:

   ```bash
   npm run preview
   ```

### Docker Deployment

1. **Build the Docker image:**

   ```bash
   docker build -t todo-frontend .
   ```

2. **Run the container:**

   ```bash
   docker run -p 80:80 todo-frontend
   ```

   The application will be available at `http://localhost`

### Using Docker Compose

If using the provided `docker-compose.yml` at the project root:

```bash
# Start all services (frontend, backend, database)
docker-compose up

# Start in detached mode
docker-compose up -d

# Stop all services
docker-compose down
```

## Component Architecture

### App.jsx

Main application component that:

- Manages application state (tasks, loading, errors)
- Fetches tasks on mount
- Handles task creation and completion
- Renders TaskForm and TaskList components

### TaskForm

Controlled form component for creating new tasks:

- Form validation
- Loading states
- Error handling
- Clears form on successful submission

### TaskList

Displays list of tasks or appropriate messages:

- Shows loading state while fetching
- Displays empty state when no tasks
- Renders TaskCard for each task

### TaskCard

Individual task display component:

- Shows task title and description
- Provides "Done" button to complete task
- Responsive card design

### API Service

Centralized API communication layer:

- `getTasks()` - Fetch recent tasks
- `createTask(taskData)` - Create new task
- `markTaskAsCompleted(taskId)` - Mark task as done

## Testing

The application includes comprehensive unit tests for all components and services:

- **Component Tests**: Test rendering, user interactions, and prop handling
- **Service Tests**: Test API calls, error handling, and data transformation
- **Integration Tests**: Test component integration and data flow

Run tests with coverage:

```bash
npm run test:coverage
```

Coverage reports are generated in the `coverage/` directory.

## Environment Variables

| Variable       | Description          | Default                     |
| -------------- | -------------------- | --------------------------- |
| `VITE_API_URL` | Backend API base URL | `http://localhost:3000/api` |

## API Integration

The frontend expects the following API endpoints:

### GET /api/tasks

Fetches the 5 most recent incomplete tasks.

**Response:**

```json
[
  {
    "id": 1,
    "title": "Task title",
    "description": "Task description",
    "completed": false,
    "created_at": "2025-10-22T10:00:00Z"
  }
]
```

### POST /api/tasks

Creates a new task.

**Request:**

```json
{
  "title": "New task",
  "description": "Task description"
}
```

**Response:**

```json
{
  "id": 2,
  "title": "New task",
  "description": "Task description",
  "completed": false,
  "created_at": "2025-10-22T10:30:00Z"
}
```

### PATCH /api/tasks/:id/complete

Marks a task as completed.

**Response:**

```json
{
  "id": 1,
  "completed": true
}
```

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Code Quality

The project follows:

- Clean Code principles
- SOLID principles
- Component composition patterns
- Separation of concerns (components, services, styles)
- Comprehensive error handling
- Proper PropTypes validation

## Styling

- Custom CSS with CSS modules per component
- Responsive design with mobile-first approach
- Dark mode support using `prefers-color-scheme`
- Smooth transitions and animations
- Accessible color contrasts

## Performance Optimizations

- Code splitting with Vite
- Lazy loading of routes (if expanded)
- Optimized bundle size
- Gzip compression in production
- Static asset caching with Nginx

## Troubleshooting

### API Connection Issues

- Ensure backend is running and accessible
- Check `VITE_API_URL` environment variable
- Verify CORS is configured on backend

### Build Failures

- Clear node_modules: `rm -rf node_modules && npm install`
- Clear Vite cache: `rm -rf node_modules/.vite`
- Ensure Node.js version is 18+

### Test Failures

- Update test snapshots if needed
- Check console for detailed error messages
- Ensure all dependencies are installed

## Contributing

1. Follow the existing code style
2. Write tests for new features
3. Update documentation as needed
4. Ensure all tests pass before committing

## License

This project is part of a technical assessment.
