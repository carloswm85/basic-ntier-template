- [Building Up a Project](#building-up-a-project)
	- [Recap: what Uncle Bob says (at a high level)](#recap-what-uncle-bob-says-at-a-high-level)
	- [Are the “entities in the center” always “plain old class / POCO / POJO”-style?](#are-the-entities-in-the-center-always-plain-old-class--poco--pojo-style)
	- [Should a project always start “Code-First”?](#should-a-project-always-start-code-first)
	- [What do teams actually do in practice?](#what-do-teams-actually-do-in-practice)
	- [About your particular structure / “where to start”](#about-your-particular-structure--where-to-start)
	- [Answers summary](#answers-summary)
	- [Toy Example](#toy-example)
		- [1. The “naive start” (no Clean Architecture yet)](#1-the-naive-start-no-clean-architecture-yet)
		- [2. Introduce domain layer + abstractions (ports) — “version 1”](#2-introduce-domain-layer--abstractions-ports--version-1)
			- [Domain / Core layer](#domain--core-layer)
			- [Application / Use-case layer](#application--use-case-layer)
		- [3. Build the infrastructure adapters (DB, API, etc.)](#3-build-the-infrastructure-adapters-db-api-etc)
				- [Infrastructure/Persistence adapter](#infrastructurepersistence-adapter)
			- [API / Controllers (driving adapter)](#api--controllers-driving-adapter)
			- [Composition / Wiring (Bootstrap)](#composition--wiring-bootstrap)
		- [4. Evolving / refinements](#4-evolving--refinements)
		- [5. Where this illustrates your original questions](#5-where-this-illustrates-your-original-questions)
	- [Port/Adapter Vs Interface](#portadapter-vs-interface)
		- [Types of Ports](#types-of-ports)
		- [Dependency directions \& layering](#dependency-directions--layering)
		- [How they relate in practice](#how-they-relate-in-practice)
		- [Example to make it concrete](#example-to-make-it-concrete)
		- [Where Interfaces are defined (vs implementations)](#where-interfaces-are-defined-vs-implementations)
		- [Some subtle points / trade-offs](#some-subtle-points--trade-offs)
		- [Summary](#summary)

---

# Building Up a Project

## Recap: what Uncle Bob says (at a high level)

In Bob’s “The Clean Architecture” article, the key ideas:

- You organize your code in concentric layers (Entities → Use Cases (Application) → Interface Adapters → Frameworks & Drivers). ([blog.cleancoder.com][1])
- The Dependency Rule: **code dependencies only point inwards** (outer layers depend on inner, not vice versa). ([blog.cleancoder.com][1])
- Entities (core business rules) are the most inner, and should not depend on anything else. ([blog.cleancoder.com][1])
- Higher-level layers adapt outer concerns (UI, DB, external APIs) into forms usable by the inner layers. ([blog.cleancoder.com][1])
- When crossing boundaries (say, from the UI layer into use cases, or from use cases into DB), you pass data in forms convenient for the _inner_ layer. (E.g. the UI should adapt to the form that the use case or entity expects.) ([blog.cleancoder.com][1])

Bob’s article is more conceptual than prescriptive about exact implementation. Many practical decisions are left to you (e.g. how “entities” concretely look, how you persist, etc.).

So when you ask whether _all_ entities in the center are “POCOs” etc., the answer is: It depends on how strictly you interpret Bob, and how much you lean on pragmatism.

---

## Are the “entities in the center” always “plain old class / POCO / POJO”-style?

“POCO” (Plain Old CLR Object) is a term from the .NET world; “POJO” is the Java equivalent: classes with fields/properties and behavior, without framework dependencies.

The ideal (in a "clean" architecture) is:

- **Yes, the core entity classes should have no dependencies on outer concerns** (no ORM annotations, no DB code, no web code). This is in line with the “no dependencies inward” rule.
- They can and often _should_ encapsulate **business logic and invariants** (not just data). That is, ideally, your domain model is _rich_ (not anemic).
- You may use value objects, domain services, aggregates, etc., in that core.

So in many clean/DDD-inspired implementations, the inner “Entities / Domain” layer is indeed pure, with minimal dependencies.

However, some caveats / nuances:

- Sometimes people _do_ put annotations (e.g. JPA in Java, or EF attributes in C#) on domain classes, accepting a small coupling to the persistence framework. That is technically bending the ideal.
- In some contexts (e.g. simpler CRUD-heavy apps), entities become “dumb data holders” (anemic domain), and the logic lives elsewhere. This is less “pure” domain-driven, but more pragmatic.
- If your language/framework supports interface-based annotations or external mapping (e.g. using mapping classes), you can preserve the purity better.

So the short answer: most correct Clean Architecture implementations aim for pure (POCO-like) entity classes. But you will find many codebases that compromise for convenience (e.g. use ORM attributes, change tracking, lazy-loading, etc.).

---

## Should a project always start “Code-First”?

“Code-First” typically means you begin by modeling your domain in code, and let the persistence layer (via migrations or ORM) generate the schema. The alternative is Database-First (model from existing DB), or Hybrid.

From a Clean Architecture perspective:

- Starting Code-First fits more naturally with the idea that your _domain model is primary_, and persistence is secondary/adaptive. You let the domain drive the structure, then you adapt the DB to it.
- But it's not mandatory or always possible: sometimes you already have an existing database or legacy constraints, so you must adapt your domain to match database design.

In practice:

- If you are greenfield (starting a new project), many teams _do_ prefer Code-First (or Domain-First) design because it aligns well with having domain as the core, and seeing persistence as an implementation detail.
- In projects with existing DBs, or when there is a strong DB-driven culture (e.g. in enterprise, with DBAs controlling schemas), teams often do Database-First or hybrid approaches.

So, no, a project does _not always_ start with Code-First, but starting domain-first / code-first is a compatible, often preferred approach in Clean Architecture.

---

## What do teams actually do in practice?

Here is a sketch of how many professional teams balance the ideals with the realities:

1. **Start small and incrementally**

   - At first, you might not fully enforce all layers. You may start with a simpler layering (e.g. domain + persistence + API) and refactor later.
   - Early on, you often accept a slight coupling (say, an ORM annotation) for speed, then extract and decouple when needed.

2. **Define clear module boundaries / interfaces early**

   - Even if you don’t fully isolate everything immediately, you define interfaces (e.g. repository interfaces, service ports) that let you swap implementations later.
   - You ensure the inner layers only depend on these abstractions (interfaces) and know nothing about the outside.

3. **Use Dependency Injection and Inversion of Control**

   - Outer layers (infrastructure, UI) implement interfaces to talk to inner layers.
   - You pass concrete implementations (e.g. DB repository) into use-case classes via DI.

4. **Mapping / Translating boundary objects**

   - You typically map between DTOs / request objects / persistence models and domain entities.
   - For example, the infrastructure adapter will convert from DB row / ORM object to your domain entity (or graph).
   - That way, your domain logic works in its own “language,” insulated from the persistence or API representation.

5. **Acceptance of some “leakage / bending”**

   - In real systems, you might see domain objects carrying some helper methods (e.g. simple validation), or some annotations creeping in for convenience.
   - Some teams adopt hybrid approaches: pure domain for complex parts, simpler CRUD areas tolerating leakage.

6. **Iterative evolution**

   - Over time, as features grow, you refactor to move logic inward, decouple, isolate changes, and split modules / bounded contexts.

7. **Feature-based (Slice) structure**

   - Some teams organize code by _feature slices_ (vertical slices) rather than strictly by layer. Within each slice, you still respect the directional dependency, but group the things that change together. This helps reduce context switching overhead. (This is a common “pragmatic evolution” approach.) ([James Hickey][2])

So your ideal model is your north star, but pragmatism guides how fast/strict you get there.

---

## About your particular structure / “where to start”

You shared a link to a “base structure” (which I couldn’t fully view due to access restrictions). But assuming you have a skeleton with layers (entities, use cases, adapters, frameworks), here’s how I would approach:

1. **Identify your core domain / business rules first**

   - What are the main entities, aggregates, invariants and behaviors?
   - Sketch their APIs (methods) and relationships, independent of persistence or UI.

2. **Design the interfaces (ports) for interaction**

   - What operations (use cases) will your system support? Define interfaces for those (input ports, output ports).
   - What repository-like abstractions are needed (e.g. `IUserRepository`, `IOrderRepository`)?

3. **Implement domain / use-case layer doing "in-memory" or stubs**

   - For early prototyping, you can implement the repository interfaces with in-memory collections. This lets you test your domain and use cases without worrying about DB.
   - This is a variant of “persistence abstraction from the start.”

4. **Wire up adapters (infrastructure) later**

   - Create the real DB / ORM / external API adapters that implement the repository interfaces.
   - Write the mapping logic from DB/ORM entities to your domain object graph.

5. **Integrate the UI / API / controllers**

   - Controllers or presenters call use-case input ports, pass in domain or DTO objects (depending on style).
   - You may map incoming HTTP/JSON DTOs → domain command objects → use case.

6. **Iterate & refactor**

   - As you discover coupling or inefficiency, pull logic inward or push interface abstractions outward.
   - Break modules into smaller bounded contexts if needed.

By going from inner-to-outer (domain first, then adapters) you align with the dependency rule and avoid having outer concerns leak inward from the start.

---

## Answers summary

| Question                                         | My considered answer                                                                                                                                                             |
| ------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Are all center entities POCO / pure classes?** | Ideally yes, with no dependencies on frameworks or infrastructure. But many real codebases compromise and gradually refactor toward purity.                                      |
| **Must a project always start Code-First?**      | No. It’s often preferable (domain-first / code-first) for greenfield, but legacy or DB-driven contexts may force alternative approaches.                                         |
| **What do teams do day-to-day?**                 | A mix: start with simpler coupling, incrementally enforce interfaces, use DI, map boundaries, evolve toward cleaner separation, sometimes use feature slices to reduce friction. |

---

## Toy Example

- Toy example (in C#-style / pseudo-C#) that walks you from a minimal beginning toward a Clean / Hexagonal / Ports-and-Adapters structure. The goal is not to be perfect or fully industrial, but to illustrate the evolution and trade-offs.
- Showing how you might structure “entities”, “ports”, “adapters”, and show the evolution into infrastructure.

### 1. The “naive start” (no Clean Architecture yet)

Suppose you’re building a small “Tasks / Todo” app.

You might begin with something like:

```csharp
public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Done { get; set; }
}

public class TaskService
{
    private MyDbContext _db;

    public TaskService(MyDbContext db)
    {
        _db = db;
    }

    public Task CreateTask(string title)
    {
        var t = new Task { Id = Guid.NewGuid(), Title = title, Done = false };
        _db.Tasks.Add(t);
        _db.SaveChanges();
        return t;
    }

    public void MarkDone(Guid taskId)
    {
        var t = _db.Tasks.Find(taskId);
        t.Done = true;
        _db.SaveChanges();
    }

    public List<Task> GetAll()
    {
        return _db.Tasks.ToList();
    }
}
```

And your ASP.NET controller or API layer would call `TaskService`.
This works, but it entangles domain logic, persistence, ORM, etc. The class `Task` might have ORM attributes, and `TaskService` is tightly coupled to `MyDbContext`.

To move toward Clean Architecture, we want to peel apart concerns and introduce abstraction.

---

### 2. Introduce domain layer + abstractions (ports) — “version 1”

#### Domain / Core layer

Move your core types into a separate project (assembly) that has _no dependencies on ORM, infrastructure, or framework_. In that, you define:

```csharp
namespace Domain
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public bool Done { get; private set; }

        private Task() {}  // for ORM or infrastructure use, or factory

        public Task(Guid id, string title)
        {
            Id = id;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Done = false;
        }

        public void MarkDone()
        {
            if (Done) throw new InvalidOperationException("Already done");
            Done = true;
        }
    }
}
```

Note:

- The domain class encapsulates behavior (`MarkDone`) and invariants (e.g. you can’t mark done twice).
- It doesn’t know anything about DB, ORM, etc.

Also define **ports / repository interfaces** (these are abstractions the domain or application layer will depend on):

```csharp
namespace Domain
{
    public interface ITaskRepository
    {
        Task GetById(Guid id);
        void Add(Task task);
        void Update(Task task);
        IEnumerable<Task> GetAll();
    }
}
```

This is the “outgoing port” (driven port) from the core: the core says “I need to load/save tasks,” but it doesn’t know _how_.

#### Application / Use-case layer

You can also have a “use cases” or “application” layer (could be the same assembly or one above domain). There you define the operations your system supports, building on domain + ports:

```csharp
namespace Application
{
    public class CreateTaskRequest
    {
        public string Title { get; set; }
    }

    public class CreateTaskResponse
    {
        public Guid TaskId { get; set; }
    }

    public class MarkTaskDoneRequest
    {
        public Guid TaskId { get; set; }
    }

    public interface ITaskUseCases
    {
        CreateTaskResponse Create(CreateTaskRequest req);
        void MarkDone(MarkTaskDoneRequest req);
        IEnumerable<Domain.Task> GetAll();
    }

    public class TaskUseCases : ITaskUseCases
    {
        private readonly Domain.ITaskRepository _repo;

        public TaskUseCases(Domain.ITaskRepository repo)
        {
            _repo = repo;
        }

        public CreateTaskResponse Create(CreateTaskRequest req)
        {
            var task = new Domain.Task(Guid.NewGuid(), req.Title);
            _repo.Add(task);
            return new CreateTaskResponse { TaskId = task.Id };
        }

        public void MarkDone(MarkTaskDoneRequest req)
        {
            var task = _repo.GetById(req.TaskId);
            if (task == null) throw new Exception("Not found");
            task.MarkDone();
            _repo.Update(task);
        }

        public IEnumerable<Domain.Task> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
```

Notice:

- The application layer depends on **domain** (for `Task` and `ITaskRepository`) but not on infrastructure or frameworks.
- The I/O (API, DB) is not here; this is pure logic + orchestration.

At this point, you can write _unit tests_ of the application layer by supplying a “fake / in-memory” implementation of `ITaskRepository`. You can test business logic without touching the DB.

---

### 3. Build the infrastructure adapters (DB, API, etc.)

Now you build the outer layers which implement the ports, and which depend on domain/application but not vice versa.

##### Infrastructure/Persistence adapter

E.g. you use Entity Framework (EF) or any ORM.

```csharp
// In project Infrastructure (which references Domain)
public class EfTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Done { get; set; }
}

public class TaskDbContext : DbContext
{
    public DbSet<EfTask> Tasks { get; set; }
}

// Adapter implementing the domain port
public class EfTaskRepository : Domain.ITaskRepository
{
    private readonly TaskDbContext _db;

    public EfTaskRepository(TaskDbContext db)
    {
        _db = db;
    }

    public Domain.Task GetById(Guid id)
    {
        var ef = _db.Tasks.Find(id);
        if (ef == null) return null;
        return new Domain.Task(ef.Id, ef.Title) { /* reflect Done */ };
        // Or create a mapping or a private constructor that sets Done flag.
    }

    public void Add(Domain.Task t)
    {
        var ef = new EfTask { Id = t.Id, Title = t.Title, Done = false };
        _db.Tasks.Add(ef);
        _db.SaveChanges();
    }

    public void Update(Domain.Task t)
    {
        var ef = _db.Tasks.Find(t.Id);
        ef.Done = t.Done;
        _db.SaveChanges();
    }

    public IEnumerable<Domain.Task> GetAll()
    {
        return _db.Tasks.Select(e => new Domain.Task(e.Id, e.Title) { /* map Done */ });
    }
}
```

This adapter depends on EF / DbContext etc., and implements the domain’s `ITaskRepository`. It lives in an outer layer.

#### API / Controllers (driving adapter)

In your web / API project (topmost), you write a controller that handles HTTP, maps requests to your use-case interface, and maps domain results back to response DTOs.

```csharp
[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly Application.ITaskUseCases _useCases;

    public TaskController(Application.ITaskUseCases useCases)
    {
        _useCases = useCases;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateTaskRequest req)
    {
        var resp = _useCases.Create(req);
        return CreatedAtAction(nameof(GetAll), new { id = resp.TaskId }, resp);
    }

    [HttpPatch("{id}/done")]
    public IActionResult MarkDone(Guid id)
    {
        _useCases.MarkDone(new MarkTaskDoneRequest { TaskId = id });
        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tasks = _useCases.GetAll();
        // Optionally map Domain.Task → DTO
        return Ok(tasks.Select(t => new {
            Id = t.Id,
            Title = t.Title,
            Done = t.Done
        }));
    }
}
```

This controller depends on the application interface, and the application depends on the domain interface, but there is no dependency going inward on EF or HTTP frameworks.

#### Composition / Wiring (Bootstrap)

You need a composition root (in your API or host project) which wires everything together (dependency injection):

```csharp
services.AddDbContext<TaskDbContext>(...);
services.AddScoped<Domain.ITaskRepository, EfTaskRepository>();
services.AddScoped<Application.ITaskUseCases, Application.TaskUseCases>();
```

Thus when the controller needs `ITaskUseCases`, DI gives it `TaskUseCases`, which gets the `ITaskRepository` implementation, etc.

---

### 4. Evolving / refinements

As your app grows, you can refine and reorganize:

- **DTOs / boundary objects** — mapping in and out at the adapter layers.
- **Validation, errors, domain events, policies** inside domain/use-case layers.
- **Interface adapters**: you might insert an “adapter” layer between application and infrastructure and between application and framework, to isolate mapping logic. (This is what Bob calls “Interface Adapters”).
- **Splitting modules or bounded contexts**: tasks, users, etc., might become separate modules.
- **Testing**: Unit test domain and application layers; integration tests on adapters.

Also, as complexity grows, sometimes small “leakages” are accepted (e.g. an ORM helper, lazy loading, or mapping attributes) so long as risk is contained and refactoring is feasible.

---

### 5. Where this illustrates your original questions

- **Center entities as “plain / pure classes”**: yes, in the example, `Domain.Task` is pure, without EF or HTTP dependencies.
- **Project starting code-first**: in the toy example, we designed domain first (i.e. code-first), then built the DB adapter. That is the direction aligned with Clean Architecture.
- **Incremental approach**: you might start with a simpler coupling (e.g. put repository logic in the same project) and then later extract adapters and layers once complexity demands it.

---

## Port/Adapter Vs Interface

Let’s define the terms first as they are commonly used, so we have a shared vocabulary.

| Term                                  | Meaning / Role                                                                                                                                                                                                                                                                                          |
| ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Core / Domain / Application Logic** | The inner parts of your system: business rules, entities, use cases. Should be independent of external concerns (DB, UI, frameworks).                                                                                                                                                                   |
| **Port**                              | An abstraction that defines _how_ the core expects to communicate _with_ something external (either that something pushing requests in, or something the core pushing requests out). Ports are often expressed as interfaces (or abstract base classes). They live in inner layers (domain / use case). |
| **Adapter**                           | A concrete implementation of a port. Adapters translate between the external detail (DB, REST, message queue, UI, etc.) and what the core expects. Adapters live in outer layers (infrastructure, interface adapters).                                                                                  |
| **Interface definition**              | The “port” itself: what methods exist, what types they use (DTOs or domain types), what the contract is. Usually “interface” in OOP, or equivalent in other paradigms.                                                                                                                                  |
| **Implementation of interfaces**      | The adapter: actual classes/modules that fulfill the contract, using concrete technologies (ORM, HTTP, etc.).                                                                                                                                                                                           |

### Types of Ports

Ports are often categorized based on direction of interaction:

1. **Inbound / Driving / Primary Ports**

   - These are how external actors drive use of the application core. For example, a web request, a CLI command, a message from a broker.
   - They define operations the core offers (e.g. “CreateOrder”, “GetReport”). Often mapped to “use case interfaces” in Clean Architecture.

2. **Outbound / Driven / Secondary Ports**

   - These are how the core _calls out_ to external systems it depends on (e.g. to persist data, send notifications, call external APIs).
   - The core defines the interface (port) for what it expects an external dependency to do.

---

### Dependency directions & layering

A central rule in Clean Architecture / Ports & Adapters / Hexagonal is **Dependency Inversion** / **Dependency Rule**: inner layers should not depend on outer layers or concrete technologies. Instead:

- **Inner defines interfaces (ports) that express needs**.
- **Outer implements those interfaces** (adapters).
- The core (use cases, domain) depends only on the interfaces, not on details of implementation.

Thus:

- Interface definitions (ports) live closer to the core / inner layers.
- Implementations live in outer / infrastructure / adapter layers.

---

### How they relate in practice

Putting all together, here’s how the pieces hang together in a real system:

```terminal
[External/UI / Frameworks etc.]   <-- Adapter (Primary / Inbound)
         |
         v
[Primary Port (Interface)]        <-- defined in Use Case layer
         |
         v
[Use Case / Application Core]      <-- contains business logic; uses outbound ports
         |
         v
[Outbound Port (Interface)]        <-- defined in Use Case or Domain layer
         |
         v
[Adapter for Persistence / External API etc.]  <-- Secondary / Driven Adapter
```

- The UI (or external) adapter “implements” (or “uses”) the **primary port** interface to invoke use cases.
- The core calls the **outbound port** interfaces to require external services (e.g. for saving data).
- The actual implementations for outbound ports are adapters in infrastructure.

---

### Example to make it concrete

Imagine you have to build a feature: “Register User”.

- **Primary (Inbound) Port**: an interface `IUserRegistrationUseCase` defined in the application layer. It has a method like `Register(UserRegistrationRequest req)`.
- **Adapter for inbound**: a REST controller, command-line command, or UI form processor which receives the request, constructs `UserRegistrationRequest`, and calls `IUserRegistrationUseCase.Register(...)`.
- **Outbound Ports** defined by the core: e.g. `IUserRepository`, `IEmailSender`.
- **Adapters for outbound**: Concrete classes like `UserRepositoryEF` (using Entity Framework, SQL), or `SmtpEmailSender`, which implement those interfaces.
- **Dependency wiring**: In your composition root / dependency injection configuration, you plug in the adapter implementations for the outbound ports, and plug the use-case implementation and inbound adapter.

---

### Where Interfaces are defined (vs implementations)

This is often a point of confusion. The rule of thumb is:

- **Define the interface (port)** as _close as possible to the code that uses it_, but in an inner layer, so that inner layer depends only on the abstraction.
  - For outbound ports: the domain or use-case classes that need a repository or external service define the interface.
  - For inbound ports: the application/use-case layer defines the interface for the actions the core exposes.
- **Implement the interface** (the adapter) in the outer/infrastructure layers, which depend on external tech.

So, for example, your `UseCase` class uses `IUserRepository`, which is defined in the same (inner) layer as UseCase. The `Infrastructure` layer has class `UserRepositoryEf : IUserRepository`.

---

### Some subtle points / trade-offs

- **Granularity**: How many ports do you define? For every single external service? For every use case? Sometimes you group. Too many tiny interfaces/adapters can add friction.
- **DTOs / Data models crossing boundaries**: Interfaces often accept or return data types. Sometimes these are domain entities; sometimes request/response DTOs. You’ll have mapping in adapters.
- **Where to put mapping logic**: Between layers (core/adapters). Adapters often convert external models → domain models, and vice versa.
- **Leakage / convenience**: Sometimes you’ll see domain entities decorated with persistence annotations (say `@Column` or EF attributes) for convenience. That is a compromise (violates pure separation), but many teams accept certain compromises for simpler development, at cost of less isolation.

---

### Summary

Putting this all together:

- The _ports_ are **interfaces / contracts** specifying what the inner system needs (output ports) or what it offers (input ports).
- The _interface definitions_ are these ports: they say “this method exists, this data flows, this contract must be honored.”
- The _implementation of interfaces_ are the adapters: concrete classes that know about external details (DB, network, UI) and adapt them to the core, or adapt core’s calls to the external systems.
- The core _depends on_ the interface definitions, not the implementations, which live outside.
- Adapters _depend on_ the interface definitions, implementing them, but core stays ignorant about which adapter is used (injected via DI or some wiring).

---

[1]: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html "Clean Coder Blog"
[2]: https://www.jamesmichaelhickey.com/clean-architecture/?utm_source=chatgpt.com "Clean Architecture Disadvantages - James Hickey"
