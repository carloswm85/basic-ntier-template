Concrete CQRS examples around a “user” use case.

## Example 1 – Create user (command) vs get user (query)

- **Command side (write)** – changes state:

  ```csharp
  // Command
  public sealed record CreateUserCommand(
      string Email,
      string Password,
      string DisplayName);

  // Command handler
  public sealed class CreateUserCommandHandler
  {
      private readonly IUserRepository _users;
      private readonly IPasswordHasher _hasher;

      public CreateUserCommandHandler(IUserRepository users, IPasswordHasher hasher)
      {
          _users = users;
          _hasher = hasher;
      }

      public async Task<Guid> Handle(CreateUserCommand command, CancellationToken ct)
      {
          var hashed = _hasher.Hash(command.Password);
          var user = new User(command.Email, hashed, command.DisplayName);

          await _users.AddAsync(user, ct);
          await _users.UnitOfWork.SaveChangesAsync(ct);

          return user.Id; // just an Id or small result, not a big read model
      }
  }
  ```

- **Query side (read)** – does not change state:

  ```csharp
  // Query
  public sealed record GetUserByIdQuery(Guid Id);

  // DTO used only for reads
  public sealed record UserDetailsDto(
      Guid Id,
      string Email,
      string DisplayName,
      DateTime CreatedAt);

  // Query handler
  public sealed class GetUserByIdQueryHandler
  {
      private readonly IUserReadStore _readStore;

      public GetUserByIdQueryHandler(IUserReadStore readStore)
      {
          _readStore = readStore;
      }

      public async Task<UserDetailsDto?> Handle(GetUserByIdQuery query, CancellationToken ct)
      {
          var user = await _readStore.Users
              .Where(u => u.Id == query.Id)
              .Select(u => new UserDetailsDto(u.Id, u.Email, u.DisplayName, u.CreatedAt))
              .SingleOrDefaultAsync(ct);

          return user; // pure read, no updates
      }
  }
  ```

Notice:

- The **command handler** takes a `CreateUserCommand` and returns only what is needed to confirm the write (e.g., `Guid` or a small result object).
- The **query handler** takes a `GetUserByIdQuery` and returns a **read‑optimized DTO**, never modifying data.

## Example 2 – Controller wiring in an n‑tier app

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ICommandBus _commandBus;
    private readonly IQueryBus _queryBus;

    public UsersController(ICommandBus commandBus, IQueryBus queryBus)
    {
        _commandBus = commandBus;
        _queryBus = queryBus;
    }

    // Command endpoint: POST /api/users
    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> Create([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(request.Email, request.Password, request.DisplayName);
        var newUserId = await _commandBus.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = newUserId },
            new CreateUserResponse { Id = newUserId });
    }

    // Query endpoint: GET /api/users/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDetailsDto>> GetById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var user = await _queryBus.Send(query);

        if (user is null) return NotFound();
        return Ok(user);
    }
}
```

- `CreateUserRequest` is the **HTTP contract**; it gets mapped to `CreateUserCommand` (write path).
- `GetUserByIdQuery` drives the **read path**, returning a DTO tailored for reading.

These examples illustrate how CQRS splits “do something” (commands) from “get information” (queries).
