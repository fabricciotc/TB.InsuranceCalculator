# TB.InsuranceCalculator

Insurance premium calculator for a company that insures the employees of its corporate clients.

The API stores employee data and returns a prorated premium based on a pricing model and a
prorating method. It is a public API: there is no authentication or authorization layer.

## Stack

- .NET 10 / ASP.NET Core controllers
- Entity Framework Core with the InMemory provider
- OpenAPI + [Scalar](https://scalar.com) as the API front end (no Swagger UI)
- C# records for all request and response contracts

## Architecture

Layered solution with a basic DDD approach: business rules live in the domain model,
not in the controllers.

```
src/
  TB.Insurance.Domain          Entities, value objects, pricing and prorating rules
  TB.Insurance.Application     Use cases and contracts
  TB.Insurance.Infrastructure  EF Core persistence and repository implementations
  TB.Insurance.Api             HTTP endpoints, DI composition, Scalar
```

Dependency direction: `Api -> Infrastructure -> Application -> Domain`.

## Running

```bash
dotnet run --project src/TB.Insurance.Api
```

The API reference is served at `http://localhost:5299/scalar/v1`.

## Endpoints

| Method | Route                     | Description                                     |
| ------ | ------------------------- | ----------------------------------------------- |
| GET    | `/api/health`             | Service liveness probe                          |
| POST   | `/api/employees`          | Saves employee data and returns the employee id |
| GET    | `/api/premium`            | Returns the prorated premium                     |

## Pricing models

Full annual premium before prorating:

- `FlatRate`: 1000 USD for every member.
- `AgeRated`: age band multiplied by 100, e.g. ages 0-9 pay `age * 100`, 10-19 pay `age * 200`,
  20-29 pay `age * 300`, and so on.
- `GenderAgeRated`: same as `AgeRated` for males and for females under 18. Females aged 18 or
  older are multiplied by 1.5.

## Prorating methods

- `ByDays`: `fullPremium / 365 * remainingDays`
- `ByMonths`: `fullPremium / 12 * remainingMonths`

Remaining time is measured from the member's addition date to the policy end date.
