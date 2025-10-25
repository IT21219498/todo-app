# 🚀 Quick Start Guide - Todo Task Application

This guide will help you get the complete application running in minutes.

## Prerequisites

Choose one of the following:

### Option 1: Docker (Recommended ⭐)

- Docker Desktop installed
- Docker Compose installed

### Option 2: Manual Setup

- Node.js 18+ and npm
- .NET 8 SDK
- MySQL 8.0+

## 🐳 Option 1: Docker Setup (Easiest)

### 1. Clone and Navigate

```bash
git clone <repository-url>
cd todo-app
```

### 2. Start Everything

```bash
docker-compose up --build
```

This single command will:

- Build frontend (React + Vite)
- Build backend (.NET 8 API)
- Start MySQL database
- Initialize database with sample data
- Connect all services

### 3. Access the Application

After containers start (takes 2-3 minutes first time):

- **Frontend**: http://localhost
- **Backend API**: http://localhost:3000
- **API Docs**: http://localhost:3000/swagger
- **Health Check**: http://localhost:3000/health

### 4. Test the Application

The database comes with 5 sample tasks:

1. Buy books
2. Clean home
3. Takehome assignment
4. Play Cricket
5. Help Saman

Try:

- Creating a new task
- Marking a task as "Done"
- Watching it disappear from the list

### 5. Stop Everything

```bash
docker-compose down
```

To remove data and start fresh:

```bash
docker-compose down -v
```

## 🛠️ Option 2: Manual Setup

### Step 1: Database Setup

**Start MySQL:**

```bash
# Using Docker
docker run --name todo-db \
  -e MYSQL_ROOT_PASSWORD=rootpassword \
  -e MYSQL_DATABASE=tododb \
  -e MYSQL_USER=todouser \
  -e MYSQL_PASSWORD=todopassword \
  -p 3306:3306 \
  -d mysql:8.0

# Wait for database to start
sleep 5

# Initialize database
psql -h localhost -U todouser -d tododb -f DB/init.sql
```

OR install MySQL locally and create database:

```sql
CREATE DATABASE tododb;
CREATE USER 'todouser'@'localhost' IDENTIFIED BY 'todopassword';
GRANT ALL PRIVILEGES ON tododb.* TO 'todouser'@'localhost';
```

### Step 2: Backend Setup

```bash
cd backend/TodoApp.Api

# Restore dependencies
dotnet restore

# Run migrations (creates tables)
dotnet ef database update

# Start backend
dotnet run
```

Backend will be available at: http://localhost:3000

### Step 3: Frontend Setup

Open a new terminal:

```bash
cd frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

Frontend will be available at: http://localhost:5173

## ✅ Verification

### Check if Everything is Running

**1. Database:**

```bash
docker ps | grep todo-db
# OR
psql -h localhost -U todouser -d tododb -c "SELECT COUNT(*) FROM task;"
```

**2. Backend:**

```bash
curl http://localhost:3000/health
# Should return: {"status":"healthy","timestamp":"..."}
```

**3. Frontend:**
Open browser to http://localhost or http://localhost:5173

### Test the API Directly

```bash
# Get tasks
curl http://localhost:3000/api/tasks

# Create task
curl -X POST http://localhost:3000/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Task","description":"Test Description"}'

# Mark task as completed (replace {id} with actual ID)
curl -X PATCH http://localhost:3000/api/tasks/1/complete
```

## 🧪 Running Tests

### Frontend Tests

```bash
cd frontend
npm test                    # Run once
npm run test:coverage       # With coverage report
```

**Expected:** 31 tests passing ✅

### Backend Tests

```bash
cd backend
dotnet test                 # Run all tests
dotnet test --logger "console;verbosity=detailed"  # Detailed output
```

**Expected:** 40+ tests passing ✅

## 📊 Viewing API Documentation

With backend running, visit:

```
http://localhost:3000/swagger
```

You can:

- See all endpoints
- Try out API calls
- View request/response schemas
- Test authentication (if added)

## 🔍 Troubleshooting

### Port Already in Use

**Problem:** Port 3000, 5173, or 5432 already in use

**Solution:**

```bash
# Find process using port
lsof -i :3000    # Mac/Linux
netstat -ano | findstr :3000    # Windows

# Kill process or change port in configuration
```

### Database Connection Failed

**Problem:** Backend can't connect to database

**Solutions:**

1. Verify MySQL is running:

   ```bash
   docker ps | grep mysql
   ```

2. Check connection string in `backend/TodoApp.Api/appsettings.json`

3. Test connection:
   ```bash
   psql -h localhost -U todouser -d tododb
   ```

### Frontend Can't Reach Backend

**Problem:** API calls from frontend failing

**Solutions:**

1. Check backend is running on port 3000
2. Verify CORS settings in `backend/TodoApp.Api/Program.cs`
3. Check `frontend/.env` has correct API URL:
   ```
   VITE_API_URL=http://localhost:3000/api
   ```

### Docker Build Fails

**Problem:** Docker build or compose fails

**Solutions:**

1. Clean Docker:

   ```bash
   docker system prune -a
   docker volume prune
   ```

2. Rebuild without cache:

   ```bash
   docker-compose build --no-cache
   docker-compose up
   ```

3. Check Docker resources (memory/disk)

### Tests Failing

**Frontend:**

```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
npm test
```

**Backend:**

```bash
cd backend
dotnet clean
dotnet restore
dotnet test
```

## 📱 Using the Application

### Creating a Task

1. Fill in the "Add a Task" form:

   - **Title**: Required (max 100 characters)
   - **Description**: Optional (max 500 characters)

2. Click "Add" button

3. New task appears at the top of the list

### Marking Task as Done

1. Find the task in the list
2. Click the "Done" button on the right
3. Task disappears from the list (it's now completed)

### Notes

- Only the 5 most recent **incomplete** tasks are shown
- Completed tasks are removed from the UI
- Tasks are ordered by creation date (newest first)

## 🔐 Default Credentials

### Database

- **Host:** localhost
- **Port:** 5432
- **Database:** tododb
- **Username:** todouser
- **Password:** todopassword

**⚠️ Change these for production!**

## 📁 Important Files

### Configuration Files

- `frontend/.env` - Frontend environment variables
- `backend/TodoApp.Api/appsettings.json` - Backend configuration
- `docker-compose.yml` - Multi-container setup

### Connection Strings

Update if needed:

- Frontend: `frontend/.env` → `VITE_API_URL`
- Backend: `backend/TodoApp.Api/appsettings.json` → `ConnectionStrings:DefaultConnection`

## 🎯 What's Next?

### Development

- Modify frontend components in `frontend/src/components/`
- Add API endpoints in `backend/TodoApp.Api/Controllers/`
- Update business logic in `backend/TodoApp.Api/Services/`

### Testing

- Add frontend tests in `frontend/src/__tests__/`
- Add backend tests in `backend/TodoApp.Tests/`

### Deployment

- Use provided Dockerfiles
- Configure environment variables
- Set up CI/CD pipeline

## 📚 Additional Resources

- [Frontend README](frontend/README.md) - React app documentation
- [Backend README](backend/README.md) - .NET API documentation
- [Main README](README.md) - Complete project overview

## 💡 Tips

1. **First Time Setup:**

   - Use Docker Compose for easiest setup
   - Let it complete fully before accessing

2. **Development:**

   - Frontend has hot reload
   - Backend needs restart on code changes
   - Database persists data between restarts

3. **Testing:**

   - Run tests before committing
   - Check test coverage regularly
   - Write tests for new features

4. **Production:**
   - Change default passwords
   - Use environment variables
   - Enable HTTPS
   - Configure proper CORS

## ✅ Success Checklist

- [ ] All services start without errors
- [ ] Can access frontend in browser
- [ ] Can view Swagger documentation
- [ ] Health check returns "healthy"
- [ ] Can create a new task
- [ ] Can mark task as done
- [ ] All frontend tests pass (31)
- [ ] All backend tests pass (40+)

## 🆘 Need Help?

1. Check error logs:

   ```bash
   docker-compose logs -f
   ```

2. Review documentation in component folders

3. Check health endpoints:

   - Backend: `http://localhost:3000/health`

4. Verify database:
   ```bash
   docker exec -it todo-db psql -U todouser -d tododb -c "SELECT * FROM task;"
   ```

---

**Ready to build amazing todo lists! 🎉**
