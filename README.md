# Inovola Weather API

A simple ASP.NET Core Web API built as a technical task for the **.NET Developer** position at **Inovola**.

---

## Architecture Overview

The solution follows **Onion Architecture**, ensuring clear separation of concerns and inward dependency flow.

- **Domain** contains core business entities and contracts.
- **Application** contains use cases and business logic.
- **Infrastructure** handles external concerns (database, JWT, caching).
- **API** acts as the composition root and HTTP entry point.

---

## Folder Structure

```
Inovola.Task
│
├── Inovola.Api
│ ├── Endpoints
│ │ ├── AuthEndpoints.cs
│ │ └── WeatherEndpoints.cs
│ ├── Program.cs
│ ├── appsettings.json
│ └── weather.db
│
├── Inovola.Application
│ ├── DTOs
│ │ └── Auth
│ │ ├── AuthResponse.cs
│ │ ├── LoginRequest.cs
│ │ └── RegisterRequest.cs
│ ├── Interfaces
│ │ ├── ICacheService.cs
│ │ └── ITokenService.cs
│ ├── Services
│ │ ├── AuthService.cs
│ │ └── WeatherAppService.cs
│ ├── Validators
│ │ └── Auth
│ │ ├── LoginRequestValidator.cs
│ │ └── RegisterRequestValidator.cs
│
├── Inovola.Domain
│ ├── Entities
│ │ ├── User.cs
│ │ └── WeatherInfo.cs
│ ├── Interfaces
│ │ ├── IUserRepository.cs
│ │ └── IWeatherService.cs
│
├── Inovola.Infrastructure
│ ├── Auth
│ │ └── JwtTokenService.cs
│ ├── Caching
│ │ └── MemoryCacheService.cs
│ ├── Persistence
│ │ ├── AppDbContext.cs
│ │ └── Repositories
│ │ └── UserRepository.cs
│ ├── Weather
│ │ └── MockWeatherService.cs
│ ├── Migrations
│ │ ├── InitialCreate.cs
│ │ └── AppDbContextModelSnapshot.cs
│ └── DependencyInjection.cs
│
├── Inovola.Tests
│ └── Inovola.Application.Tests
│ ├── Auth
│ │ └── AuthServiceTests.cs
│ └── Weather
│ └── WeatherAppServiceTests.cs
│
├── Inovola.WeatherTask.sln
└── README.md
```

---

## Features

- User Registration & Login
- JWT-based Authentication
- Protected Weather Endpoint
- Mocked Weather Data
- In-memory Caching (5 minutes)
- SQLite database with EF Core migrations
- Unit tests for application services

---

## Authentication

- JWT tokens are generated on successful registration/login.
- Standard claims are used:
  - `sub` → User ID
  - `email` → User Email
- Protected endpoints require a valid Bearer token.

---

## Weather Endpoint

```
GET /api/weather?city={cityName}
```

- Requires authentication
- Returns mocked weather data
- Results are cached per city for 5 minutes

---

## Database

- SQLite is used for simplicity and ease of setup.
- `Users` table includes:
  - Auto-increment primary key
  - Unique index on Email
  - Database-level constraints

EF Core migrations are located in the **Infrastructure** project.

---

## How to Run

1. Restore dependencies
2. Apply database migrations:
   ```bash
   dotnet ef database update
   ```
3. Run the API project
4. Use Postman to test the endpoints

---

## Testing

- Unit tests cover:
  - `AuthService`
  - `WeatherAppService`
- External dependencies are mocked.
- Tests focus on behavior, not implementation details.

Run tests using:
```bash
dotnet test
```

---

## Design Notes

- Password hashing is intentionally simplified for demonstration purposes.
- Weather data is mocked to keep the focus on architecture and flow.
- The solution avoids unnecessary abstractions to remain practical and readable.

---

## Summary

This project demonstrates:
- Clean layering and separation of concerns
- Practical authentication flow
- Sensible trade-offs appropriate for a technical task
