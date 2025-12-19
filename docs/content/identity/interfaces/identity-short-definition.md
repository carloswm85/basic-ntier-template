- [Identity API Services – Minimal Core Subset](#identity-api-services--minimal-core-subset)
  - [1. Core Identity Services](#1-core-identity-services)
  - [2. Token \& Session Management](#2-token--session-management)
  - [3. Role \& Claims Management](#3-role--claims-management)
  - [4. Security \& Verification](#4-security--verification)
  - [5. Audit \& Monitoring](#5-audit--monitoring)
    - [✅ **Minimal Identity Core Total**](#-minimal-identity-core-total)
    - [🧱 **What You Can Add Later (Optional Phases)**](#-what-you-can-add-later-optional-phases)
- [Interface To Identity Built Ins Mappings](#interface-to-identity-built-ins-mappings)
  - [🧭 Why Mapping Matters](#-why-mapping-matters)
  - [⚙️ The Mapping Table](#️-the-mapping-table)
  - [🧩 Which Are Wrappers vs. Custom](#-which-are-wrappers-vs-custom)
  - [💡 The Benefit](#-the-benefit)

---

# Identity API Services – Minimal Core Subset

- **Streamlined, production-ready subset** of Identity API — the **essential interfaces**.
- These cover all critical identity functions (auth, user, token, password, roles) while deferring advanced or specialized features for later phases.

## 1. Core Identity Services

Essential for authentication, user management, and secure credential handling.
Implements the minimal identity lifecycle: registration, login, profile updates, and password operations.

| #   | Interface Name           | Description                                                                 | Estimated Methods |
| --- | ------------------------ | --------------------------------------------------------------------------- | ----------------- |
| 1.1 | `IUserService`           | Basic user CRUD, profile queries, and state changes (enable/disable)        | 8–10              |
| 1.2 | `IAuthenticationService` | Handles login/logout, credential validation, and token issuance integration | 6–8               |
| 1.3 | `IRegistrationService`   | Registers users, sends confirmation email, activates account                | 4–6               |
| 1.4 | `IPasswordService`       | Password hashing, validation, reset workflow                                | 5–7               |
|     |                          | **Total Methods**                                                           | **23–31**         |

---

## 2. Token & Session Management

Required for issuing and managing JWT access/refresh tokens in secure APIs.

| #   | Interface Name         | Description                             | Estimated Methods |
| --- | ---------------------- | --------------------------------------- | ----------------- |
| 2.1 | `ITokenService`        | Create, validate, and revoke JWT tokens | 6–8               |
| 2.2 | `IRefreshTokenService` | Generate and rotate refresh tokens      | 4–6               |
|     |                        | **Total Methods**                       | **10–14**         |

---

## 3. Role & Claims Management

Minimum for enforcing role-based and claims-based authorization.

| #   | Interface Name  | Description                             | Estimated Methods |
| --- | --------------- | --------------------------------------- | ----------------- |
| 3.1 | `IRoleService`  | Assign/remove roles, query user roles   | 6–8               |
| 3.2 | `IClaimService` | Retrieve, add, and validate user claims | 5–6               |
|     |                 | **Total Methods**                       | **11–14**         |

---

## 4. Security & Verification

Includes only the most common verification step for user confirmation.
Additional layers (2FA, phone, security stamps) can be added later.

| #   | Interface Name              | Description                                 | Estimated Methods |
| --- | --------------------------- | ------------------------------------------- | ----------------- |
| 4.1 | `IEmailVerificationService` | Send and validate email verification tokens | 4–5               |
|     |                             | **Total Methods**                           | **4–5**           |

---

## 5. Audit & Monitoring

Basic logging for login events and failed authentication attempts.
Advanced analytics, device tracking, or geolocation can be deferred.

| #   | Interface Name  | Description                                    | Estimated Methods |
| --- | --------------- | ---------------------------------------------- | ----------------- |
| 5.1 | `IAuditService` | Record authentication and user activity events | 4–6               |
|     |                 | **Total Methods**                              | **4–6**           |

---

### ✅ **Minimal Identity Core Total**

| Category                   | Estimated Total Methods |
| -------------------------- | ----------------------- |
| Core Identity Services     | 23–31                   |
| Token & Session Management | 10–14                   |
| Role & Claims Management   | 11–14                   |
| Security & Verification    | 4–5                     |
| Audit & Monitoring         | 4–6                     |
| **Grand Total**            | **52–70**               |

---

### 🧱 **What You Can Add Later (Optional Phases)**

| Phase   | Deferred Area                        | Adds Features Like                                  |
| ------- | ------------------------------------ | --------------------------------------------------- |
| Phase 2 | Two-Factor Auth & Phone Verification | 2FA, OTP, backup codes                              |
| Phase 3 | Account Lockout & Recovery           | Lockout policy, recovery flow                       |
| Phase 4 | External Auth                        | OAuth/OIDC, Google/Facebook login                   |
| Phase 5 | Advanced Security                    | Device trust, impersonation, API keys, audit trails |

---

# Interface To Identity Built Ins Mappings

- Correspond interfaces to built-in Identity components, and which are abstractions to be built.

---

## 🧭 Why Mapping Matters

You don’t want to reinvent everything Identity already gives you.
Mapping lets you:

- **Reuse** ASP.NET Core Identity’s secure, tested functionality
- **Wrap** it behind your cleaner interfaces so your API remains framework-agnostic
- **Replace or extend** behavior later without breaking API consumers

---

## ⚙️ The Mapping Table

| Your Interface              | ASP.NET Core Identity Equivalent               | Role / Notes                                                                                                                       |
| --------------------------- | ---------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| `IUserService`              | `UserManager<TUser>`                           | Wraps CRUD, user queries, and status changes (`FindByIdAsync`, `UpdateAsync`, `DeleteAsync`, etc.)                                 |
| `IAuthenticationService`    | `SignInManager<TUser>` + `UserManager<TUser>`  | Handles login (`PasswordSignInAsync`), logout, and token issuance integration                                                      |
| `IRegistrationService`      | `UserManager<TUser>` + custom logic            | Uses `CreateAsync` and adds email confirmation and onboarding workflows                                                            |
| `IPasswordService`          | `UserManager<TUser>`                           | Uses methods like `CheckPasswordAsync`, `ChangePasswordAsync`, and `ResetPasswordAsync`                                            |
| `ITokenService`             | (Custom — ASP.NET Identity doesn’t handle JWT) | Generates and validates JWT/refresh tokens; built separately or via libraries like `Microsoft.AspNetCore.Authentication.JwtBearer` |
| `IRefreshTokenService`      | (Custom)                                       | Manages refresh tokens (Identity doesn’t store these by default)                                                                   |
| `IRoleService`              | `RoleManager<TRole>`                           | Wraps role CRUD and user-role assignments                                                                                          |
| `IClaimService`             | `UserManager<TUser>`                           | Uses `AddClaimAsync`, `GetClaimsAsync`, `RemoveClaimAsync`                                                                         |
| `IEmailVerificationService` | `UserManager<TUser>` + custom token provider   | Uses `GenerateEmailConfirmationTokenAsync` and `ConfirmEmailAsync`                                                                 |
| `IAuditService`             | (Custom)                                       | Identity doesn’t log events by default; you implement this for login attempts and security logs                                    |

---

## 🧩 Which Are Wrappers vs. Custom

| Type                                     | Interfaces                                                                                                                                         |
| ---------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Wrappers (built on ASP.NET Identity)** | `IUserService`, `IAuthenticationService`, `IRegistrationService`, `IPasswordService`, `IRoleService`, `IClaimService`, `IEmailVerificationService` |
| **Custom (you must implement)**          | `ITokenService`, `IRefreshTokenService`, `IAuditService`                                                                                           |

---

## 💡 The Benefit

By wrapping built-ins:

- You keep your API **framework-neutral** (you could swap out ASP.NET Identity for Duende IdentityServer, or even a microservice later).
- You **simplify testing** (mock your interfaces instead of dealing with EF Core or Identity internals).
- You gain **clean separation of concerns** — your controllers depend on `IAuthenticationService`, not `SignInManager`.
