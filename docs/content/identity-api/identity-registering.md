- [1️⃣ `AddIdentityApiEndpoints<TUser>`](#1️⃣-addidentityapiendpointstuser)
- [2️⃣ `AddIdentity<TUser, TRole>`](#2️⃣-addidentitytuser-trole)
- [3️⃣ `AddDefaultIdentity<TUser>`](#3️⃣-adddefaultidentitytuser)
- [Decision guide](#decision-guide)
- [Architectural takeaway](#architectural-takeaway)

---

### High-level difference

| Feature                  | `AddIdentityApiEndpoints<TUser>`           | `AddIdentity<TUser, TRole>` | `AddDefaultIdentity<TUser>` |
| ------------------------ | ------------------------------------------ | --------------------------- | --------------------------- |
| Primary use              | **HTTP API** (Minimal APIs / SPA / mobile) | **Full Identity system**    | **MVC / Razor Pages UI**    |
| Auth style               | **Bearer tokens + cookies**                | Cookies (by default)        | Cookies                     |
| Built-in endpoints       | ✅ `/login`, `/register`, `/refresh`, etc. | ❌                          | ❌                          |
| Role support             | ❌ (claims & policies only)                | ✅                          | ❌                          |
| UI included              | ❌                                         | ❌                          | ✅ (scaffoldable UI)        |
| Minimal hosting friendly | ✅                                         | ⚠️                          | ⚠️                          |

---

## 1️⃣ `AddIdentityApiEndpoints<TUser>`

**What it is**

A **newer Identity flavor** designed for **API-first applications**.

```csharp
builder.Services.AddIdentityApiEndpoints<ApplicationUser>();
```

**What it configures**

- IdentityCore
- Bearer token authentication
- Cookie fallback (hybrid scenarios)
- Token endpoints (login, register, refresh, confirm, reset)
- Minimal API integration (`MapIdentityApi<TUser>()`)

**What it does _not_ include**

- ❌ Roles
- ❌ UI
- ❌ MVC helpers

**Best for**

- SPA (Angular / React / Vue)
- Mobile clients
- Clean separation of frontend and backend

**Key implication**

You use:

- **Claims**
- **Policies**
- Not roles

Forward-thinking, modern, API-centric.

---

## 2️⃣ `AddIdentity<TUser, TRole>`

**What it is**

The **classic full Identity stack**.

```csharp
builder.Services.AddIdentity<ApplicationUser, IdentityRole>();
```

**What it configures**

- User manager
- Role manager
- Cookie authentication
- Claims, roles, policies
- Extensible authorization model

**What it does _not_ include**

- ❌ UI (unless you add Identity UI separately)
- ❌ API endpoints

**Best for**

- MVC apps
- Razor Pages
- Server-rendered apps
- Role-based authorization

**Key implication**

- Heavier
- More flexible
- Roles are first-class citizens

---

## 3️⃣ `AddDefaultIdentity<TUser>`

**What it is**

A **preconfigured convenience wrapper** around `AddIdentityCore`.

```csharp
builder.Services.AddDefaultIdentity<ApplicationUser>();
```

**What it configures**

- Cookie authentication
- Default UI
- Email confirmation defaults
- Basic user management

**What it does _not_ include**

- ❌ Roles (unless `.AddRoles<TRole>()`)
- ❌ API endpoints
- ❌ Minimal API support

**Best for**

- Fast setup
- Razor Pages apps
- Traditional MVC with UI

---

## Decision guide

```
SPA / Mobile / Angular / React
        ↓
AddIdentityApiEndpoints<TUser>

MVC / Razor Pages + Roles
        ↓
AddIdentity<TUser, TRole>

Quick UI + Authentication
        ↓
AddDefaultIdentity<TUser>
```

---

## Architectural takeaway

- `AddIdentityApiEndpoints` → **Identity as a service**
- `AddIdentity<TUser, TRole>` → **Identity as infrastructure**
- `AddDefaultIdentity` → **Identity as a feature**

If you want, I can:

- Show a migration path from `AddIdentity` → API endpoints
- Explain why roles were intentionally excluded
- Provide a production-grade authorization pattern using policies only
