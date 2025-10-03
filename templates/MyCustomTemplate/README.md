# MyCustomTemplate

This is a test.

## "Clean Architecture" In This Solution Template

| Level | Clean Arch.    | Uncle Bob          | Equivalent in This Template\* | Technology             |
| ----- | -------------- | ------------------ | ----------------------------- | ---------------------- |
| 1     | Domain         | Entities           | Data, Service enums           | SQL Server             |
| 2     | Application    | Use Cases          | IRepository, Repository       | -                      |
| 3     | Infrastructure | Interface Adapters | Data, Repository              | Entity Frameworkc Core |
| 4     | Presentation   | API                | Web, Web API                  | Angular, Swagger,etc   |

\* The equivalence is approximate.
