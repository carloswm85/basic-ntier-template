- [Architectural Design: Layer Boundaries and Contracts Between Web and Service Tiers - Example: `UserCreate*`](#architectural-design-layer-boundaries-and-contracts-between-web-and-service-tiers---example-usercreate)
  - [What Contracts Are](#what-contracts-are)
  - [Where To Use Them Between Layers](#where-to-use-them-between-layers)
  - [Naming Conventions For Contracts](#naming-conventions-for-contracts)
  - [Example Naming Table](#example-naming-table)
  - [`CreateUserRequest` vs `UserViewModel`](#createuserrequest-vs-userviewmodel)
  - [What CQRS Is And Where `CreateUserCommand` Fits](#what-cqrs-is-and-where-createusercommand-fits)
  - [How To Use `CreateUserCommand`](#how-to-use-createusercommand)
  - [Compare: `CreateUserRequest`, `CreateUserResponse`, `CreateUserCommand` and `CreateUserViewModel`](#compare-createuserrequest-createuserresponse-createusercommand-and-createuserviewmodel)
  - [Flow Diagram](#flow-diagram)
  - [Further Reading](#further-reading)

---

# Architectural Design: Layer Boundaries and Contracts Between Web and Service Tiers - Example: `UserCreate*`

> Service/API contracts that define the public surface of the service layer.

- Contracts are the data shapes and message types that define layers talk to each other (API/MVC ↔ Application/Service), independently of the database or UI details.
- They are usually simple, serializable classes that carry input (commands/requests) and output (results/responses) for each use case, such as “CreateUser\*”.

## What Contracts Are

- In an n‑tier or clean architecture, contract classes are often called _DTOs_ (Data Transfer Objects) or _messages_, and they live in a project that both the Web and Service layers can reference.
- Their main goal is to isolate the inner domain and persistence model from what the outer world (controllers, clients) see, so changes in entities or EF models do not break controllers or clients as long as the contract stays compatible.

Typical patterns to consider:

- **Command/Request**: everything needed to perform an operation (e.g., create/update a user).
- **Result/Response**: what the operation returns (data plus success/error info).
- **Common result wrapper**: a `Result` or `OperationResult` type that carries `IsSuccess`, `Errors`, etc., used by multiple operations.

## Where To Use Them Between Layers

- Controllers (API or MVC) should accept and return these contract classes, not domain entities.
- The controller maps incoming HTTP JSON into a contract (e.g., `CreateUserRequest`), passes it to an application/service method, and receives a contract back (e.g., `CreateUserResponse` or `RegistrationResult`).
- The service layer can then map between contracts and domain models internally using manual mapping or tools like AutoMapper, keeping mapping logic out of controllers.

## Naming Conventions For Contracts

There is no single mandatory standard, but common, clear conventions in .NET are:

- **Suffix by role** (highly recommended):
  - Requests into the system: `CreateUserRequest`, `UpdateUserRequest`, `RegistrationRequest`.
  - Responses out of the system: `CreateUserResponse`, `RegistrationResult` or `RegistrationResponse`.
  - Nested/child DTOs: `UserDto`, `AddressDto`, etc., when they are used inside those request/response types.
- **Verb + Entity + Suffix** for operations:
  - Commands: `CreateUserCommand`, `UpdateUserCommand`, `DeleteUserCommand`.
  - Queries: `GetUserQuery`, `ListUsersQuery`.
  - This pattern is popular when using CQRS/mediator libraries such as MediatR, where commands/queries are internal “messages”, and controller‑level contracts can still be named `CreateUserRequest/Response` if you want to keep external vs internal clear.
- **PascalCase class names, PascalCase properties**, following usual .NET conventions, with the JSON serializer taking care of camelCase on the wire if needed.

## Example Naming Table

| I/O    | Purpose                         | Suggested name                                 | Notes                                                        |
| ------ | ------------------------------- | ---------------------------------------------- | ------------------------------------------------------------ |
| Input  | API input to create user        | `CreateUserRequest`                            | Used in MVC/API action parameter.                            |
| Input  | Internal app command (optional) | `CreateUserCommand`                            | Input to command handler in service/CQRS layer.              |
| Output | API output after create         | `CreateUserResponse` or `CreateUserResult`     | Contains new `UserId`, maybe a summary DTO.                  |
| Input  | API input to register user      | `RegistrationRequest`                          | Similar to create user but maybe with extra fields.          |
| Output | API output for registration     | `RegistrationResult` or `RegistrationResponse` | Includes success flag, tokens, etc.                          |
| Both   | Reusable operation wrapper      | `Result` or `OperationResult<T>`               | Wraps success/error around commands and queries.             |
| Output | Nested user data                | `UserDto`                                      | Returned inside responses or query results; not UI-specific. |

Clean rule:

- “Everything that crosses the Web boundary uses the `Request`/`Response`/`Result` suffix; everything that is a CQRS message inside the application uses `Command`/`Query`.”
- Keep these contract types in a `Contracts` (or `Application.Contracts`) project/folder, separate from `Domain` and `Persistence`, and avoid putting EF or business logic inside them.

---

## `CreateUserRequest` vs `UserViewModel`

- `CreateUserRequest`: DTO/contract used at the API boundary to represent the HTTP payload for creating a user (what the client sends into the API method). It should contain only what is needed to perform the create operation and be stable over time as part of your public API.
- `UserViewModel`: a model tailored for rendering views (e.g., MVC Razor) or UI state, often combining multiple pieces of data or UI-specific fields like select‑list items, labels, or validation metadata.
- In APIs you often don’t need a ViewModel; you keep a `CreateUserRequest` contract and a `UserDto` for outputs. In MVC with Razor, you might map between `UserViewModel` (for the view) and `CreateUserRequest` or a domain model in the controller.

## What CQRS Is And Where `CreateUserCommand` Fits

- **CQRS (Command Query Responsibility Segregation)** is a pattern where operations that change state (commands: create/update/delete) are separated from those that read state (queries: get/list/search).
- A **command** object like `CreateUserCommand` represents the intent to change the system (e.g., “create this user with these properties”) and is handled by a command handler in the application layer.
- In a typical flow: controller receives `CreateUserRequest` → maps it to `CreateUserCommand` → sends it to a command handler → handler runs business logic and persistence → returns a `Result`/`CreateUserResponse`.

## How To Use `CreateUserCommand`

- Keep `CreateUserCommand` inside the application/service layer, not directly exposed to the outside world.
- Map from external contract to command:
  - API/MVC layer: `CreateUserRequest` → `CreateUserCommand`.
  - Application layer: command handler executes, then returns `Result` plus any output DTO (`CreateUserResponse`).
- Commands should be focused on writes: they should not return big read models; usually they return only success/failure and identifiers or small summaries.

---

## Compare: `CreateUserRequest`, `CreateUserResponse`, `CreateUserCommand` and `CreateUserViewModel`

> Consider including `CreateUserResult`

| Aspect                 | `CreateUserRequest`                                                            | `CreateUserResponse`                                        | `CreateUserCommand`                                                           | `CreateUserViewModel`                                     |
| ---------------------- | ------------------------------------------------------------------------------ | ----------------------------------------------------------- | ----------------------------------------------------------------------------- | --------------------------------------------------------- |
| **Layer**              | API/Presentation Layer                                                         | Service/Application Layer                                   | Service/Application Layer                                                     | MVC/Presentation Layer                                    |
| **Purpose**            | Receive data from HTTP client                                                  | Return result of creation operation                         | Trigger business logic operation                                              | Bind form data, display to view                           |
| **Validation**         | Data annotations, model binding                                                | N/A (output only)                                           | Business rules validation                                                     | Data annotations, model binding, UI-specific validation   |
| **Coupling**           | Coupled to web framework (ASP\.NET)                                            | Framework-agnostic                                          | Framework-agnostic                                                            | Coupled to MVC framework (ASP\.NET MVC)                   |
| **Changes when**       | API contract changes                                                           | What clients need to know about creation changes            | Business requirements change                                                  | UI/UX requirements change                                 |
| **Example attributes** | `[Required]`, `[EmailAddress]`, `[FromBody]`                                   | Clean, no framework attributes                              | Clean, no framework attributes                                                | `[Required]`, `[Display]`, `[Compare]`, `SelectList`      |
| **Used by**            | API Controllers/Endpoints                                                      | Returned to Controllers (both MVC and API)                  | Service layer methods                                                         | MVC Controllers and Razor Views                           |
| **Used with**          | `[FromBody] CreateUserRequest`                                                 | `return Ok(response)` or `return View("Success", response)` | Passed to service methods like `CreateUserAsync(CreateUserCommand command)`   | `@model CreateUserViewModel` in Razor                     |
| **Data flow**          | HTTP Client → CreateUserRequest → API Controller → CreateUserCommand → Service | Service → CreateUserResponse → Controller → Client/View     | Browser Form → CreateUserViewModel → Controller → CreateUserCommand → Service | Browser Form ↔ Controller ↔ View                          |
| **Content**            | Pure data fields needed for creation                                           | Minimal result data (UserId, UserName, Email, CreatedAt)    | Pure data fields needed for business operation                                | Display logic, validation for UI, dropdown lists, helpers |

**In practice:** Your controller maps `CreateUserRequest` → `CreateUserCommand`, keeping layers decoupled.

**NOTES:**

- You should use `CreateUserCommand` between MVC layer and service layer.
- MVC layer uses `CreateUserRequest` to receive HTTP data
- MVC layer translates it to `CreateUserCommand`
- Service layer only knows about `CreateUserCommand`
- Service layer stays decoupled from web concerns

## Flow Diagram

```terminal
MVC Flow:
Browser → CreateUserViewModel → MVC Controller → CreateUserCommand → Service
                                                                         ↓
                                MVC Controller ← CreateUserResponse ← Service
                                      ↓
                                 Razor View (display success/redirect)

API Flow:
Client → CreateUserRequest → API Controller → CreateUserCommand → Service
                                                                      ↓
                            API Controller ← CreateUserResponse ← Service
                                  ↓
                            JSON Response to Client
```

---

## Further Reading

- [CQRS Example](cqrs-example.md)
