- [Project Example Details](#project-example-details)
  - [Detailed Explanations by Layer and File](#detailed-explanations-by-layer-and-file)
    - [1. **MyApp.Domain/** (Core - innermost layer)](#1-myappdomain-core---innermost-layer)
    - [2. **MyApp.Application/** (Use Cases)](#2-myappapplication-use-cases)
    - [3. **MyApp.Infrastructure/** (External concerns)](#3-myappinfrastructure-external-concerns)
    - [4. **MyApp.WebApi/** (Presentation/Entry point)](#4-myappwebapi-presentationentry-point)
  - [Layer Comments](#layer-comments)
    - [Additional Notes](#additional-notes)
  - [Files Per Layer](#files-per-layer)
    - [1. DOMAIN LAYER - MyApp.Domain](#1-domain-layer---myappdomain)
    - [2. APPLICATION LAYER - MyApp.Application](#2-application-layer---myappapplication)
    - [3. INFRASTRUCTURE LAYER - MyApp.Infrastructure](#3-infrastructure-layer---myappinfrastructure)
    - [4. WEB API LAYER - MyApp.WebApi](#4-web-api-layer---myappwebapi)

---

# Project Example Details

---

## Detailed Explanations by Layer and File

### 1. **MyApp.Domain/** (Core - innermost layer)

- **`Entities/User.cs` - Business entity with identity**

  - Represents a domain object with a unique identifier (ID)
  - Contains business logic and invariants (validation rules)
  - Mutable state tracked throughout its lifecycle
  - Example: User has Id, Name, Email properties and ChangeEmail() method

- **`Entities/Order.cs` - Aggregate root**

  - Entry point for a cluster of related entities (Order contains OrderItems)
  - Enforces consistency boundaries - all changes go through the root
  - Controls access to child entities
  - Example: Order manages its OrderItems collection and calculates total

- **`ValueObjects/Email.cs` - Immutable value object**

  - Defined by its attributes, not identity (two emails with same value are equal)
  - Cannot be changed after creation (immutable)
  - Contains validation logic in constructor
  - Example: Email validates format and throws if invalid

- **`ValueObjects/Money.cs` - Currency and amount**
  - Combines related attributes into a single concept
  - Prevents primitive obsession (using decimal everywhere)
  - Ensures currency operations are type-safe
  - Example: Money(100, "USD") with Add() method that validates matching currencies

### 2. **MyApp.Application/** (Use Cases)

- **`Interfaces/IUserRepository.cs` - Port for persistence**

  - Defines contract for data access without implementation details
  - Allows Domain/Application to remain agnostic of database choice
  - Enables dependency inversion principle
  - Example: Interface with GetById(), Add(), Update() methods

- **`UseCases/CreateUserUseCase.cs` - Application service**

  - Coordinates a single user action/workflow
  - Orchestrates domain objects and repositories
  - Contains application-specific business rules
  - Example: Validates uniqueness, creates User entity, saves via repository

- **`UseCases/PlaceOrderUseCase.cs` - Business workflow**
  - Implements complex multi-step business process
  - Transactions and cross-cutting concerns
  - May involve multiple repositories and domain services
  - Example: Validates inventory, creates Order, updates stock, sends confirmation

### 3. **MyApp.Infrastructure/** (External concerns)

- **`Persistence/ApplicationDbContext.cs` - EF Core context**

  - ORM configuration for database mapping
  - Defines how entities are persisted
  - Contains DbSet properties and model configurations
  - Example: DbContext with entity configurations, migrations, connection string

- **`Repositories/UserRepository.cs` - Implements IUserRepository**

  - Concrete implementation of the persistence interface
  - Contains actual database access code (LINQ, SQL)
  - Translates between domain entities and database models
  - Example: Uses DbContext to query, map results to User entities

- **`Repositories/OrderRepository.cs` - Data access implementation**
  - Handles persistence of complex aggregates
  - Manages relationships and loading strategies
  - May include caching or performance optimizations
  - Example: Loads Order with related OrderItems using Include()

### 4. **MyApp.WebApi/** (Presentation/Entry point)

- **`Controllers/UsersController.cs` - REST endpoints**

  - Handles HTTP verbs (GET, POST, PUT, DELETE)
  - Routes requests to appropriate use cases
  - Returns HTTP status codes and responses
  - Example: POST endpoint calls CreateUserUseCase, returns 201 Created

- **`DTOs/CreateUserRequest.cs` - API contract**

  - Defines input shape for API consumers
  - Data validation attributes ([Required], [EmailAddress])
  - Decouples external API from internal domain models
  - Example: Contains Name and Email properties for user creation

- **`DTOs/UserResponse.cs` - Serialization model**
  - Defines output format for API responses
  - May flatten or transform domain entities for consumption
  - Prevents over-posting and controls what's exposed
  - Example: Contains Id, Name, Email (excludes PasswordHash)

---

## Layer Comments

**Domain Layer** - Pure business logic with no external dependencies:

- Entities with encapsulated state and business rules
- Value objects with immutability and validation
- Rich domain models that protect invariants

**Application Layer** - Orchestrates domain logic:

- Repository interfaces (ports) for dependency inversion
- Use cases that coordinate business workflows
- Command/Result patterns for clear contracts

**Infrastructure Layer** - Technical implementations:

- EF Core DbContext with entity configurations
- Repository implementations with actual data access
- Proper use of Include() for loading related entities

**Web API Layer** - Presentation and HTTP concerns:

- Controllers that delegate to use cases
- DTOs for API contracts with validation attributes
- Response mapping to decouple domain from API

### Additional Notes

Notice how dependencies flow inward:

- WebApi references Application
- Infrastructure implements Application interfaces
- Domain has zero dependencies

This makes the core business logic testable and framework-agnostic.

---

## Files Per Layer

### 1. DOMAIN LAYER - MyApp.Domain

```cs
// --- Entities/User.cs ---
namespace MyApp.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private User() { } // For EF Core

    public User(string name, Email email)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        CreatedAt = DateTime.UtcNow;

        Validate();
    }

    public void ChangeEmail(Email newEmail)
    {
        if (newEmail == null)
            throw new ArgumentNullException(nameof(newEmail));

        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty", nameof(newName));

        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name is required");
        if (Name.Length < 2 || Name.Length > 100)
            throw new ArgumentException("Name must be between 2 and 100 characters");
    }
}
```

```cs
// --- Entities/Order.cs ---
namespace MyApp.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime OrderDate { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // For EF Core

    public Order(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Status = OrderStatus.Pending;
        OrderDate = DateTime.UtcNow;
    }

    public void AddItem(string productName, Money price, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify a confirmed order");

        var item = new OrderItem(Id, productName, price, quantity);
        _items.Add(item);
    }

    public void RemoveItem(Guid itemId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify a confirmed order");

        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
            _items.Remove(item);
    }

    public Money GetTotal()
    {
        if (!_items.Any())
            return new Money(0, "USD");

        return _items
            .Select(i => i.GetSubtotal())
            .Aggregate((a, b) => a.Add(b));
    }

    public void Confirm()
    {
        if (!_items.Any())
            throw new InvalidOperationException("Cannot confirm an empty order");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new InvalidOperationException("Cannot cancel a shipped order");

        Status = OrderStatus.Cancelled;
    }
}

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; }
    public Money Price { get; private set; }
    public int Quantity { get; private set; }

    private OrderItem() { } // For EF Core

    public OrderItem(Guid orderId, string productName, Money price, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        Quantity = quantity;
    }

    public Money GetSubtotal()
    {
        return Price.Multiply(Quantity);
    }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Cancelled
}
```

```cs
// --- ValueObjects/Email.cs ---
namespace MyApp.Domain.ValueObjects;

public class Email : IEquatable<Email>
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        value = value.Trim().ToLowerInvariant();

        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object? obj) => Equals(obj as Email);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
```

```cs
// --- ValueObjects/Money.cs ---
namespace MyApp.Domain.ValueObjects;

public class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required", nameof(currency));

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot add {other.Currency} to {Currency}");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot subtract {other.Currency} from {Currency}");

        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor, Currency);
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount:F2} {Currency}";
}
```

### 2. APPLICATION LAYER - MyApp.Application

```cs
// --- Interfaces/IUserRepository.cs ---
namespace MyApp.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Email email, CancellationToken cancellationToken = default);
}
```

```cs
// --- Interfaces/IOrderRepository.cs ---
namespace MyApp.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}
```

```cs
// --- UseCases/CreateUserUseCase.cs ---
namespace MyApp.Application.UseCases;

public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CreateUserResult> ExecuteAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validate uniqueness
        var email = new Email(command.Email);
        var exists = await _userRepository.ExistsAsync(email, cancellationToken);

        if (exists)
        {
            return new CreateUserResult
            {
                Success = false,
                Error = "User with this email already exists"
            };
        }

        // Create domain entity
        var user = new User(command.Name, email);

        // Persist
        await _userRepository.AddAsync(user, cancellationToken);

        return new CreateUserResult
        {
            Success = true,
            UserId = user.Id
        };
    }
}

public record CreateUserCommand(string Name, string Email);

public class CreateUserResult
{
    public bool Success { get; set; }
    public Guid? UserId { get; set; }
    public string? Error { get; set; }
}
```

```cs
// --- UseCases/PlaceOrderUseCase.cs ---
namespace MyApp.Application.UseCases;

public class PlaceOrderUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;

    public PlaceOrderUseCase(
        IUserRepository userRepository,
        IOrderRepository orderRepository)
    {
        _userRepository = userRepository;
        _orderRepository = orderRepository;
    }

    public async Task<PlaceOrderResult> ExecuteAsync(
        PlaceOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null)
        {
            return new PlaceOrderResult
            {
                Success = false,
                Error = "User not found"
            };
        }

        // Create order aggregate
        var order = new Order(command.UserId);

        // Add items
        foreach (var item in command.Items)
        {
            var price = new Money(item.Price, "USD");
            order.AddItem(item.ProductName, price, item.Quantity);
        }

        // Confirm order
        order.Confirm();

        // Persist
        await _orderRepository.AddAsync(order, cancellationToken);

        return new PlaceOrderResult
        {
            Success = true,
            OrderId = order.Id,
            Total = order.GetTotal().Amount
        };
    }
}

public record PlaceOrderCommand(
    Guid UserId,
    List<OrderItemCommand> Items);

public record OrderItemCommand(
    string ProductName,
    decimal Price,
    int Quantity);

public class PlaceOrderResult
{
    public bool Success { get; set; }
    public Guid? OrderId { get; set; }
    public decimal Total { get; set; }
    public string? Error { get; set; }
}
```

### 3. INFRASTRUCTURE LAYER - MyApp.Infrastructure

```cs
// --- Persistence/ApplicationDbContext.cs ---
namespace MyApp.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Email");
            });

            entity.HasIndex(u => u.Email.Value)
                .IsUnique();
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Status)
                .HasConversion<string>();

            entity.HasMany(typeof(OrderItem))
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);

            entity.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            entity.OwnsOne(oi => oi.Price, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(18,2)");

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3);
            });
        });
    }
}
```

```cs
// --- Repositories/UserRepository.cs ---
namespace MyApp.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email.Value, cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.Value == email.Value, cancellationToken);
    }
}
```

```cs
// --- Repositories/OrderRepository.cs ---
namespace MyApp.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<List<Order>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
```

### 4. WEB API LAYER - MyApp.WebApi

```cs
// --- Controllers/UsersController.cs ---
namespace MyApp.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;
    private readonly IUserRepository _userRepository;

    public UsersController(
        CreateUserUseCase createUserUseCase,
        IUserRepository userRepository)
    {
        _createUserUseCase = createUserUseCase;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var response = users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email.Value,
            CreatedAt = u.CreatedAt
        }).ToList();

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
            return NotFound();

        var response = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email.Value,
            CreatedAt = user.CreatedAt
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var command = new CreateUserCommand(request.Name, request.Email);
        var result = await _createUserUseCase.ExecuteAsync(command, cancellationToken);

        if (!result.Success)
            return BadRequest(new { error = result.Error });

        var user = await _userRepository.GetByIdAsync(result.UserId!.Value, cancellationToken);

        var response = new UserResponse
        {
            Id = user!.Id,
            Name = user.Name,
            Email = user.Email.Value,
            CreatedAt = user.CreatedAt
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}
```

```cs
// --- Controllers/OrdersController.cs ---
namespace MyApp.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly PlaceOrderUseCase _placeOrderUseCase;
    private readonly IOrderRepository _orderRepository;

    public OrdersController(
        PlaceOrderUseCase placeOrderUseCase,
        IOrderRepository orderRepository)
    {
        _placeOrderUseCase = placeOrderUseCase;
        _orderRepository = orderRepository;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<OrderResponse>>> GetByUserId(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId, cancellationToken);

        var response = orders.Select(o => new OrderResponse
        {
            Id = o.Id,
            UserId = o.UserId,
            Status = o.Status.ToString(),
            OrderDate = o.OrderDate,
            Total = o.GetTotal().Amount,
            Items = o.Items.Select(i => new OrderItemResponse
            {
                Id = i.Id,
                ProductName = i.ProductName,
                Price = i.Price.Amount,
                Quantity = i.Quantity,
                Subtotal = i.GetSubtotal().Amount
            }).ToList()
        }).ToList();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> PlaceOrder(
        [FromBody] PlaceOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var items = request.Items.Select(i =>
            new OrderItemCommand(i.ProductName, i.Price, i.Quantity)).ToList();

        var command = new PlaceOrderCommand(request.UserId, items);
        var result = await _placeOrderUseCase.ExecuteAsync(command, cancellationToken);

        if (!result.Success)
            return BadRequest(new { error = result.Error });

        var order = await _orderRepository.GetByIdAsync(result.OrderId!.Value, cancellationToken);

        var response = new OrderResponse
        {
            Id = order!.Id,
            UserId = order.UserId,
            Status = order.Status.ToString(),
            OrderDate = order.OrderDate,
            Total = order.GetTotal().Amount,
            Items = order.Items.Select(i => new OrderItemResponse
            {
                Id = i.Id,
                ProductName = i.ProductName,
                Price = i.Price.Amount,
                Quantity = i.Quantity,
                Subtotal = i.GetSubtotal().Amount
            }).ToList()
        };

        return CreatedAtAction(nameof(GetByUserId), new { userId = response.UserId }, response);
    }
}
```

```cs
// --- DTOs/CreateUserRequest.cs ---
namespace MyApp.WebApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
}
```

```cs
// --- DTOs/UserResponse.cs ---
namespace MyApp.WebApi.DTOs;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
```

```cs
// --- DTOs/PlaceOrderRequest.cs ---
namespace MyApp.WebApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class PlaceOrderRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string ProductName { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
    public decimal Price { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}
```

```cs
// --- DTOs/OrderResponse.cs ---
namespace MyApp.WebApi.DTOs;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
}

public class OrderItemResponse
{
    public Guid Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
```
