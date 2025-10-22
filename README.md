# Event Management System

**Event Management System** is a simplified  event management system developed with **ASP.NET Core Web API** and **Angular**.

---
## Technologies Used
- [.NET 9 (ASP.NET Core Web API)]
- [PostgreSQL 15+]
- Entity Framework Core
- Clean Architecture (Domain, Application, Infrastructure, API)
- CQRS
- AutoMapper
- FluentValidation
- Angular
- Docker

---

### Setup Locally Angular
```bash
git clone https://github.com/sashanazarchuk/Application.git
cd Application/frontend
npm install
ng serve
```
API will be available at:
```bash
http://localhost:4200
```

### Setup Locally Asp.Net
```bash
cd Application/backend/EventSystem.API
dotnet run
```
API will be available at:
```bash
http://localhost:5016/swagger/index.html
```
## Environment Variables 
Before running the backend locally, configure your PostgreSQL connection string.
All sensitive data (database, JWT credentials) should be stored in Environment Variables.
Example appsettings.json structure for reference (values are left empty):

```json
{
  "ConnectionStrings": {
    "sqlConnection": " "
  },
  "JwtSettings": {
    "Issuer": " ",
    "Audience": " ",
    "Secret": " ",
    "AccessTokenExpirationMinutes": 5,
    "RefreshTokenExpirationDays": 10
  }
}

```
## Run Application using Docker
**Create `.env` file** in the root folder based on `.env.example`. Fill in your own values:
```bash
# PostgreSQL
DB_SERVER=your_db_host
DB_PORT=5432
DB_USER=your_db_user
DB_PASSWORD=your_db_password
DB_NAME=your_db_name

# JWT settings
JWT_ISSUER=your_jwt_issuer
JWT_AUDIENCE=your_jwt_audience
JWT_SECRET=your_jwt_secret
JWT_ACCESSTOKENEXPIRATIONMINUTES=5
JWT_REFRESHTOKENEXPIRATIONDAYS=10
```
Build and run Docker containers:
```bash
cd Application
docker-compose up --build
```
After running, the API will be available at:
```bash
http://localhost:8080/swagger/index.html
```
Angular will be available at:
```bash
http://localhost:4200
