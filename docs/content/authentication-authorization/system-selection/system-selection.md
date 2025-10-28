- [Authentication-Authorization System Selection](#authentication-authorization-system-selection)
  - [Main Readings](#main-readings)
  - [Answer 1](#answer-1)
    - [🧩 Your scenario](#-your-scenario)
    - [🧭 Flow through the chart](#-flow-through-the-chart)
    - [✅ Recommendation](#-recommendation)
    - [⚙️ Typical Architecture](#️-typical-architecture)
  - [Answer 2](#answer-2)
    - [✅ Free/open-source identity server option](#-freeopen-source-identity-server-option)
    - [🔄 Recommended authentication \& offline-capable flow](#-recommended-authentication--offline-capable-flow)
      - [1. Identity Server](#1-identity-server)
      - [2. Client registration \& flows](#2-client-registration--flows)
      - [3. API protection](#3-api-protection)
      - [4. Offline considerations for mobile](#4-offline-considerations-for-mobile)
      - [5. Logout / revoked tokens](#5-logout--revoked-tokens)
      - [6. Implementation tech mapping](#6-implementation-tech-mapping)
  - [Final Selection: `OpenIddict`](#final-selection-openiddict)
    - [Why OpenIddict for Your Case](#why-openiddict-for-your-case)
    - [Architecture Overview](#architecture-overview)
    - [Implementation](#implementation)

---

> *Most of this content was AI generated.*

# Authentication-Authorization System Selection

## Main Readings

- Questions:
  - [Choose an identity management solution](https://learn.microsoft.com/en-us/aspnet/core/security/how-to-choose-identity-solution?view=aspnetcore-8.0)
  - [How to use Identity to secure a Web API backend for SPAs](https://)
  - From Identity API to OpenIddict:
    - [Can you use the ASP.NET Core Identity API endpoints with OpenIddict?]([https://](https://kevinchalet.com/2023/10/04/can-you-use-the-asp-net-core-identity-api-endpoints-with-openiddict/))
    - [Should you use the .NET 8 Identity API endpoints?](https://andrewlock.net/should-you-use-the-dotnet-8-identity-api-endpoints/#what-are-the-new-identity-api-endpoints-)
- Providers:
  - <https://github.com/orgs/openiddict/repositories>
    - <https://documentation.openiddict.com/guides/>
    - UI Implementations:
      - <https://github.com/openiddict/openiddict-samples>
      - <https://github.com/thomasduft/openiddict-ui>
      - <https://github.com/Nfactor26/pixel-identity>
      - <https://github.com/damienbod/AspNetCoreOpeniddict>
- Implementation examples:
  - [Implement a secure web application using nx Standalone Angular and an ASP.NET Core server](https://damienbod.com/2023/09/11/implement-a-secure-web-application-using-nx-standalone-angular-and-an-asp-net-core-server/)
    - <https://github.com/damienbod/bff-aspnetcore-angular>
    - <https://github.com/damienbod/bff-openiddict-aspnetcore-angular>
- Other solutions:
  - <https://github.com/OrchardCMS/OrchardCore>
  - <https://github.com/abpframework/abp>
- Tutorials and blog entries:
  - <https://dev.to/isaacojeda/aspnet-core-servidor-de-autenticacion-con-openid-connect-59kh>
  - <https://kevinchalet.com/2025/07/07/openiddict-7-0-is-out/>

---

## Answer 1

My question:

> Explain.
>
> ![alt](../../img/identity-management-decision-flow.png)
>
> Source: <https://learn.microsoft.com/en-us/aspnet/core/security/how-to-choose-identity-solution?view=aspnetcore-8.0>
>
> My requirements: Web application for use over the internet. The same application has a mobile version with offline capabilities.

### 🧩 Your scenario

- **Type:** Web + Mobile application
- **Availability:** Mobile version can work **offline**
- **Accessibility:** Web app runs **on the Internet**

### 🧭 Flow through the chart

1. **Do external apps access your protected APIs?**
   → ✅ **Yes**, your mobile app accesses the same APIs as your web app.

2. **Does your app have Internet access all the time?**
   → ❌ **No**, since the mobile app has offline capabilities.

3. **Are you required to store user data on your own servers?**
   → Usually **Yes**, if you manage your own backend and identity store.

   → This leads to:
   **🟠 “Self-host, installed or container, OIDC server.”**

### ✅ Recommendation

Because you have:

- Multiple clients (web + mobile),
- APIs accessed by external apps,
- Offline support (so you can’t rely solely on cloud-based identity),
- Likely control over your data and infrastructure,

👉 **Use a self-hosted OpenID Connect (OIDC) server**, such as:

- **Duende IdentityServer** (recommended for ASP.NET Core; commercial license)
- **AuthServer** or a custom **OpenIddict**-based implementation

This setup allows:

- Secure token-based authentication (JWT/OAuth2)
- Support for **offline refresh tokens** for your mobile app
- Integration with ASP.NET Core Identity (if you want local users)
- Control over data and policies

### ⚙️ Typical Architecture

| Component                   | Technology                              | Purpose                                             |
| --------------------------- | --------------------------------------- | --------------------------------------------------- |
| **Identity Provider (IDP)** | Duende IdentityServer / OpenIddict      | Issues tokens, manages users                        |
| **Web App**                 | ASP.NET Core MVC or Razor Pages         | Uses cookies or tokens                              |
| **API**                     | ASP.NET Core Web API                    | Protected via bearer tokens                         |
| **Mobile App**              | MAUI / Xamarin / Flutter / React Native | Uses access + refresh tokens, supports offline mode |

---

## Answer 2

> Question: My front end is built using Angular 17, Razor in NET Framework 4.7.2, and Flutter in mobile. I need a free to use solution
>
> Rlso reply to: Recommended authentication flow (including refresh tokens for offline use and ASP.NET Core setup)

### ✅ Free/open-source identity server option

Since you want a **free to use solution**, a good match is OpenIddict — a free, open-source OAuth 2.0 / OpenID Connect stack for .NET.

- It’s under a permissive Apache 2 license, so you can use it even in commercial projects.
- It supports building a server, a validation stack, and a client stack in .NET.
- It supports ASP.NET Core and can also be used (in part) in legacy ASP.NET 4.x apps (via OWIN) which may help with your Razor / .NET Framework 4.7.2 parts.

So you could host your own identity provider using OpenIddict, integrate all your clients (Angular web, Razor backend, Flutter mobile), rely on tokens/refresh flows for offline mobile support, and avoid license costs.

### 🔄 Recommended authentication & offline-capable flow

Given your architecture (Angular + Razor + Flutter mobile offline), here is a **suggested flow**:

#### 1. Identity Server

- Use OpenIddict to implement your own Identity Provider (IDP) in a .NET Core (or .NET 6/8) project.
- Expose the usual endpoints: `/authorize`, `/token`, `/revocation`, `/userinfo`, etc.
- Configure clients for your:

  - Angular app (SPA)
  - Razor backend (web server)
  - Flutter mobile app

#### 2. Client registration & flows

- Angular (web SPA): Use the **Authorization Code Flow with PKCE** (recommended for browser-based clients) to authenticate users.

  - After sign-in, get _id_token_ and _access_token_.
  - Use access_token to call your backend APIs.

- Razor web backend: Since it's a server-side web app, you can treat it as a confidential client: use Authorization Code Flow (server side) and possibly maintain a cookie session for UI.
- Flutter mobile (offline capable):

  - Use Authorization Code Flow with PKCE for interactive login.
  - **Support refresh tokens** so that when offline you can still refresh access tokens when the device reconnects (or use a local cache of valid session state).
  - On the device you may store a short-lived access token plus a refresh token (securely) and optionally store some offline user state (while respecting security).

#### 3. API protection

- Your backend APIs (that both the Angular web and Flutter mobile will call) should validate access tokens issued by your IDP. Use JWT bearer validation or introspection depending on setup.
- Protect endpoints accordingly: require scopes, roles, etc.

#### 4. Offline considerations for mobile

- On mobile, when offline you typically cannot call the token endpoint to refresh. The pattern:

  - On login, device receives access_token + refresh_token.
  - Access_token is short-lived (e.g., 15 minutes).
  - Refresh_token is longer (e.g., days).
  - While offline, allow the app to operate with cached credentials or locally-allowed operations until reconnect. When connectivity resumes, use refresh_token to obtain a new access_token.
  - Be careful about secure storage of refresh tokens and token revocation if lost device.

- If needed, you can implement a “device offline mode” where the mobile app uses its own local authentication state (maybe encrypting locally) and syncs later.

#### 5. Logout / revoked tokens

- Support token revocation (when device is lost or user signs out) so that refresh tokens or access tokens can be invalidated.
- Consider offline logout complications: device may be offline when user logs out elsewhere — you’ll have to handle eventual sync or token expiry.

#### 6. Implementation tech mapping

- IDP: .NET Core + OpenIddict
- Web API: .NET Core Web API (token validation)
- Angular: Use e.g. `@azure/msal-browser` or `oidc-client` library to handle OIDC flows (or a custom OIDC library).
- Flutter: Use `flutter_appauth` (or similar) to handle OIDC flows + PKCE + refresh tokens.

---

## Final Selection: `OpenIddict`

OpenIddict is an **excellent choice** for your scenario! You need a centralized identity server that can handle multiple client types, and OpenIddict is perfect for this.

### Why OpenIddict for Your Case

✅ **Multi-client support** - Perfect for your Angular, Razor (.NET Framework), and Flutter apps
✅ **Free and open-source** - Unlike Duende IdentityServer (commercial license required)
✅ **Integrates seamlessly with ASP.NET Core Identity**
✅ **Supports all OIDC flows** you'll need for different client types
✅ **Active development and good documentation**

### Architecture Overview

```terminal
┌─────────────────────────────────────┐
│   OpenIddict Server (Web API)      │
│   - ASP.NET Core Identity           │
│   - User Management                 │
│   - Token Generation                │
│   - Authorization Endpoints         │
└─────────────────────────────────────┘
           │
           │ OIDC/OAuth2
           │
    ┌──────┴──────┬──────────┬──────────┐
    │             │          │          │
┌───▼────┐  ┌────▼───┐  ┌───▼────┐  ┌──▼─────┐
│Angular │  │ Razor  │  │Flutter │  │  APIs  │
│  SPA   │  │.NET FW │  │  App   │  │Resource│
└────────┘  └────────┘  └────────┘  └────────┘
```

### Implementation

See document: [./openiddict.md](./pre-selection-openiddict.md)
