- [Project Example](#project-example)
  - [Structure Example](#structure-example)
  - [Layer Breakdown](#layer-breakdown)

---

# Project Example

---

## Structure Example

```terminal
MyApp.sln
src/
  MyApp.Domain/
    Entities/
    ValueObjects/
  MyApp.Application/
    Interfaces/
    UseCases/
  MyApp.Infrastructure/
    Persistence/
    Repositories/
  MyApp.WebApi/
    Controllers/
    DTOs/
```

---

This is a **Clean Architecture** project structure organized by layers with **dependency inversion** - inner layers don't know about outer layers.

## Layer Breakdown

**MyApp.Domain/** (Core - innermost layer)

- Contains business rules and enterprise logic
- Zero dependencies on other layers
- **Examples:**
  - `Entities/User.cs` - Business entity with identity
  - `Entities/Order.cs` - Aggregate root
  - `ValueObjects/Email.cs` - Immutable value object
  - `ValueObjects/Money.cs` - Currency and amount

**MyApp.Application/** (Use Cases)

- Orchestrates domain logic for specific application scenarios
- Depends only on Domain
- **Examples:**
  - `Interfaces/IUserRepository.cs` - Port for persistence
  - `UseCases/CreateUserUseCase.cs` - Application service
  - `UseCases/PlaceOrderUseCase.cs` - Business workflow

**MyApp.Infrastructure/** (External concerns)

- Implements technical details
- Depends on Application (implements its interfaces)
- **Examples:**
  - `Persistence/ApplicationDbContext.cs` - EF Core context
  - `Repositories/UserRepository.cs` - Implements IUserRepository
  - `Repositories/OrderRepository.cs` - Data access implementation

**MyApp.WebApi/** (Presentation/Entry point)

- Handles HTTP requests/responses
- Depends on Application and Infrastructure
- **Examples:**
  - `Controllers/UsersController.cs` - REST endpoints
  - `DTOs/CreateUserRequest.cs` - API contract
  - `DTOs/UserResponse.cs` - Serialization model

**Key principle:** Dependencies flow inward (WebApi → Infrastructure → Application → Domain), making the core business logic independent and testable.
