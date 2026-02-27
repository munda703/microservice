Wonga Auth – Microservices Architecture (Dockerized)

1. Overview
This project is a Dockerized microservices-based authentication system built using ASP.NET Core, PostgreSQL, React (Vite), and Docker Compose.
 The system consists of multiple independent services that communicate through an API Gateway.

3. Architecture
The solution follows a Microservices Architecture and includes the following services:
• API Gateway – Routes requests to internal services
• RegistrationService – Handles user registration
• UserService – Handles user login and user details
• ClientApp – React frontend application
• PostgreSQL – Database container
Each service runs in its own Docker container and communicates through Docker's internal network using service names.

5. Technologies Used
Backend:
• ASP.NET Core (.NET 8)
• Entity Framework Core
• PostgreSQL
• JWT Authentication
• FluentValidation
• Serilog
Frontend:
• React (Vite)
• Axios / Fetch API
• nginx (Production serving)
DevOps:
• Docker
• Docker Compose
• Git & GitHub

6. How the System Works
1. User registers through the frontend.
2. RegistrationService stores the user in PostgreSQL.
3. User logs in via UserService.
4. JWT token is generated and returned.
5. Frontend stores the token.
6. API Gateway validates and forwards protected requests.
7. MyDetails endpoint returns authenticated user data.
5. Running the Application (Docker Only)
   
Step 1: Clone the repository

git clone https://github.com/munda703/microservice.git
cd microservice
Step 2: Build Docker images
docker compose build
Step 3: Run the containers
docker compose up
If one of the containers is not running, start it manually using:
docker compose up <service_name> or click play button on it.
After all containers are running, open the frontend by navigating to:
http://localhost:3001 from your browser hit enter

7. Application Usage
8. 
• Open http://localhost:3001
• Register a new user
• Login with registered credentials
• After successful login, system navigates to MyDetails page
9. Important Note
10. 
Before running this microservices project, ensure that the monolithic version of the application is stopped and deleted to avoid database conflicts.
To remove old containers and volumes:
docker compose down -v
11. Source Control
12. 
This project is managed using Git and hosted on GitHub:
https://github.com/munda703/microservice.git

