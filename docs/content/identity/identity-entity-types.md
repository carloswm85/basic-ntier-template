# Entity types

The Identity model consists of the following entity types.

| Entity      | type Description                                              |
| ----------- | ------------------------------------------------------------- |
| `User`      | Represents the user.                                          |
| `Role`      | Represents a role.                                            |
| `UserClaim` | Represents a claim that a user possesses.                     |
| `UserToken` | Represents an authentication token for a user.                |
| `UserLogin` | Associates a user with a login.                               |
| `RoleClaim` | Represents a claim that's granted to all users within a role. |
| `UserRole`  | A join entity that associates users and roles.                |

- Each `User` can have many `UserClaims`.
- Each `User` can have many `UserLogins`.
- Each `User` can have many `UserTokens`.
- Each `Role` can have many associated `RoleClaims`.
- Each `User` can have many associated `Roles`, and each `Role` can be associated with many Users. This is a many-to-many relationship that requires a join table in the database. The join table is represented by the `UserRole` entity.
