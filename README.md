# Quiz

An interactive quiz API where users guess the authors of quotes. Supports two game modes and is built on Clean Architecture with ASP.NET Core 10.

## Features

- **Binary mode** — answer yes/no: is the suggested author correct?
- **Multiple choice mode** — pick the correct author from several options
- JWT authentication with refresh token support
- Score tracking across games
- Admin tools for managing quotes and users
- Auto-applied database migrations on startup
- OpenAPI docs with Scalar UI (development)

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 (Minimal APIs) |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| CQRS | MediatR 12 |
| Validation | FluentValidation 12 |
| Auth | ASP.NET Core Identity + JWT Bearer |
| Logging | Serilog |
| API Docs | OpenAPI / Scalar |

## Architecture

The solution follows Clean/Onion Architecture across five projects:

```
Quiz.Api            Minimal API endpoints, auto-discovered via IEndpoint
Quiz.Application    CQRS commands/queries, pipeline behaviors, repository abstractions
Quiz.Domain         Entities, Result pattern, domain error types
Quiz.Infrastructure EF Core DbContext, repositories, Identity, JWT token manager
Quiz.Common         Result<T>, Error types, shared constants
```

Dependencies flow inward: Api → Application → Domain ← Infrastructure.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

## Getting Started

1. **Clone the repo**

   ```bash
   git clone https://github.com/sabbam-3/Quiz-Back-End
   cd Quiz
   ```

2. **Configure settings** — edit `Quiz.Api/appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "QuizDb": "Server=.;Database=QuizDb;Trusted_Connection=True;"
     },
     "Jwt": {
       "Issuer": "quiz-api",
       "Audience": "quiz-client",
       "Secret": "",
       "ExpirationHours": 1
     }
   }
   ```

3. **Run the API**

   ```bash
   dotnet run --project Quiz.Api
   ```

   Migrations are applied automatically on startup. The API listens on:
   - HTTP: `http://localhost:5133`
   - HTTPS: `https://localhost:7083`

4. **Browse the API docs** (development only)

   ```
   http://localhost:5133/scalar
   ```

## API Overview

| Group | Description |
|---|---|
| `POST /auth/login` | Obtain JWT + refresh token |
| `POST /auth/register` | Create a new account |
| `POST /auth/refresh` | Refresh an expired JWT |
| `GET /auth/me` | Current user info |
| `GET/POST/PUT/DELETE /users` | User management (admin) |
| `GET/POST/PUT/DELETE /quotes` | Quote management (admin write) |
| `POST /games/binary` | Start a Binary quiz game |
| `POST /games/multiple-choice` | Start a Multiple Choice quiz game |
| `POST /games/{id}/answer` | Submit an answer |
| `POST /games/{id}/abandon` | Abandon an in-progress game |
| `GET /games/{id}` | Retrieve game state and score |

## Game Modes

**Binary**
**Multiple Choice** — Each question presents a quote alongside several author options. The player selects the index of the correct author.

Both modes track correctness per question and compute a final score when the game completes.
