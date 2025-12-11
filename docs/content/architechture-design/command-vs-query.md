- [Command Vs Query](#command-vs-query)
  - [1. Command](#1-command)
  - [2. Query](#2-query)
- [Key Difference (Intent-Based)](#key-difference-intent-based)
  - [Practical Rule of Thumb](#practical-rule-of-thumb)
  - [Common Mistake to Avoid](#common-mistake-to-avoid)
  - [Final takeaway](#final-takeaway)

---

# Command Vs Query

In Clean Architecture, **Command** and **Query** represent two fundamentally different intentions. This distinction is commonly formalized by **CQRS (Command Query Responsibility Segregation)**, even when you are not implementing full CQRS infrastructure.

---

## 1. Command

**Purpose:**
👉 **Change system state**

A **Command** expresses _intent to perform an action_ that **modifies data**.

**Characteristics**

- ✅ Causes side effects (create, update, delete)
- ✅ Represents a _use case_
- ✅ Validated aggressively
- ✅ May fail for business reasons
- ✅ Executed by the Service / Use Case layer
- ❌ Should not expose internal data structures

**Examples**

- `CreateUserCommand`
- `UpdatePasswordCommand`
- `DeactivateAccountCommand`

**Typical Shape**

```csharp
public sealed class CreateUserCommand
{
    public string Email { get; init; }
    public string Password { get; init; }
}
```

**Result**

- Returns a `Result`, `CommandResult`, or `OperationResult`
- Result may include an ID, status, or minimal DTO
- Never returns full aggregates

---

## 2. Query

**Purpose:**
👉 **Read system state**

A **Query** expresses a _request for information_ and **must not change data**.

**Characteristics**

- ❌ No side effects
- ✅ Idempotent
- ✅ Optimized for reading
- ✅ Often bypasses full domain logic
- ✅ Can project data directly to DTOs

**Examples**

- `GetUserByIdQuery`
- `ListUsersQuery`
- `GetUserSummaryQuery`

**Typical Shape**

```csharp
public sealed class GetUserByIdQuery
{
    public Guid UserId { get; init; }
}
```

**Result**

- Returns a ViewModel or Read DTO
- May return collections, paged data, summaries
- Can be tailored exactly to the UI needs

---

# Key Difference (Intent-Based)

| Aspect               | Command         | Query        |
| -------------------- | --------------- | ------------ |
| Intent               | Change state    | Read state   |
| Side effects         | ✅ Yes          | ❌ No        |
| Validation           | Business rules  | Minimal      |
| Execution path       | Domain-centric  | Data-centric |
| Performance priority | Correctness     | Speed        |
| Return value         | Result / Status | Data         |

---

## Practical Rule of Thumb

> - **If it changes state → Command**
> - **If it only reads → Query**

If a method both _reads and writes_, it is a **Command**, even if it returns data.

---

## Common Mistake to Avoid

❌ Returning rich domain objects from commands
✅ Commands return _outcomes_, queries return _data_

---

## Final takeaway

**Commands represent behavior.**
**Queries represent information.**

Keeping this separation sharp is what preserves Clean Architecture boundaries as the system grows.
