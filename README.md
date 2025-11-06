# 🛑 STILL UNDER DEVELOPMENT 🛑

Use branches `main` and `developer-*` for **unused** and development versions respectively.

Project template generates 6 layers:

| Level | Layers        | Classification                     | Functionality                                          | Technology/Notes                                                   | Role                                                                             |
| ----- | ------------- | ---------------------------------- | ------------------------------------------------------ | ------------------------------------------------------------------ | -------------------------------------------------------------------------------- |
| 1     | `Data`        | -                                  | Database schema, migrations, models                    | Entity Framework Core, Identity API integration                    | Define database schema, manage migrations, and entity mapping.                   |
| 2     | `Repository`  | -                                  | Data access abstraction, caching, Unit of Work Pattern | Querying interfaces                                                | Abstract data access logic to decouple the service layer from EF Core specifics. |
| 3     | `Service`     | Business logic                     | Logic, validation                                      | Business rules, support async methods                              | Implement business logic, enforce rules and validation.                          |
| 4.a   | `API`         | Request handling, Dev presentation | RESTful API, documentation                             | Swagger/OpenAPI, versioning                                        | Expose business services as RESTful endpoints.                                   |
| 4.b   | `Web.MVC`     | User presentation                  | Server-side rendering, UI logic                        | MVC design pattern, Razor syntax                                   | Traditional server-rendered UI using MVC and Razor Pages.                        |
| 4.c   | `Web.Angular` | User presentation                  | Client-side SPA                                        | Angular SPA, with strong typing (TypeScript), Rest API integration | Modern client-side SPA experience.                                               |

## Branches Under Development

`main` brach is not reciving updates as of now.

Remember this project is still under development. Yet, branches are usable. But because they are still under development, you shall use them under your own discretion:

- [developer-lightweight](https://github.com/carloswm85/basic-ntier-template/tree/developer-lightweight) - NET Core 8 **without** Identity API
    - It can be used, but it may require additional development to fit your needs.
    - Is has NOT been fully tested yet.
    - I would say it's safe to use in new project, but be aware that it has not been battle-tested.
- [developer-identity](https://github.com/carloswm85/basic-ntier-template/tree/developer-identity) - NET Core 8 and Identity API
    - This one branch is more cumbersome because of the on-going integration of Identity.
    - I would not use it.
    - I repeat, I would not use it (yet).

