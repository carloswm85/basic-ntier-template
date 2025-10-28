- [Why OpenIddict for Your Case](#why-openiddict-for-your-case)
- [Architecture Overview](#architecture-overview)
- [Implementation](#implementation)
  - [1. OpenIddict Server (Authorization Server + Resource Server)](#1-openiddict-server-authorization-server--resource-server)
  - [2. Angular 17 Client](#2-angular-17-client)
  - [3. Razor App (.NET Framework 4.7.2)](#3-razor-app-net-framework-472)
  - [4. Flutter App](#4-flutter-app)
- [Key Points](#key-points)

---

> *Most of this content was AI generated.*

OpenIddict is an **excellent choice** for your scenario! You need a centralized identity server that can handle multiple client types, and OpenIddict is perfect for this.

## Why OpenIddict for Your Case

✅ **Multi-client support** - Perfect for your Angular, Razor (.NET Framework), and Flutter apps
✅ **Free and open-source** - Unlike Duende IdentityServer (commercial license required)
✅ **Integrates seamlessly with ASP.NET Core Identity**
✅ **Supports all OIDC flows** you'll need for different client types
✅ **Active development and good documentation**

## Architecture Overview

```
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

## Implementation

### 1. OpenIddict Server (Authorization Server + Resource Server)

**Install packages:**
```bash
dotnet add package OpenIddict.AspNetCore
dotnet add package OpenIddict.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```

**Program.cs:**

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseOpenIddict(); // Important!
});

// ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// OpenIddict
builder.Services.AddOpenIddict()
    // Register Entity Framework stores
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>();
    })
    // Register ASP.NET Core components
    .AddServer(options =>
    {
        // Enable the authorization, token, userinfo and introspection endpoints
        options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetTokenEndpointUris("/connect/token")
            .SetUserinfoEndpointUris("/connect/userinfo")
            .SetIntrospectionEndpointUris("/connect/introspect");

        // Enable the flows you need
        options.AllowAuthorizationCodeFlow()
            .RequireProofKeyForCodeExchange() // PKCE for Angular & Flutter
            .AllowRefreshTokenFlow()
            .AllowClientCredentialsFlow(); // For machine-to-machine

        // Register scopes
        options.RegisterScopes(
            Scopes.OpenId,
            Scopes.Profile,
            Scopes.Email,
            "api" // Your API scope
        );

        // Register signing and encryption credentials
        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        // Register ASP.NET Core host
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough()
            .EnableStatusCodePagesIntegration();
    })
    // Register validation components (for API endpoints)
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// CORS for your clients
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200", // Angular dev
            "https://your-angular-app.com",
            "https://your-razor-app.com"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Seed clients on startup
await SeedClientsAsync(app.Services);

app.UseCors("AllowClients");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Seed OpenIddict clients
async Task SeedClientsAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync();

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

    // Angular SPA
    if (await manager.FindByClientIdAsync("angular-spa") == null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "angular-spa",
            DisplayName = "Angular SPA",
            Type = ClientTypes.Public, // No client secret for SPAs
            ConsentType = ConsentTypes.Implicit,
            RedirectUris = 
            {
                new Uri("http://localhost:4200/callback"),
                new Uri("https://your-angular-app.com/callback")
            },
            PostLogoutRedirectUris =
            {
                new Uri("http://localhost:4200/"),
                new Uri("https://your-angular-app.com/")
            },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                "scp:api"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });
    }

    // Razor App (.NET Framework 4.7.2)
    if (await manager.FindByClientIdAsync("razor-app") == null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "razor-app",
            ClientSecret = "your-secret-here", // Use secure secret
            DisplayName = "Razor Web App",
            Type = ClientTypes.Confidential, // Server-side app
            ConsentType = ConsentTypes.Implicit,
            RedirectUris =
            {
                new Uri("https://your-razor-app.com/signin-oidc")
            },
            PostLogoutRedirectUris =
            {
                new Uri("https://your-razor-app.com/signout-callback-oidc")
            },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                "scp:api"
            }
        });
    }

    // Flutter App
    if (await manager.FindByClientIdAsync("flutter-app") == null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "flutter-app",
            DisplayName = "Flutter Mobile App",
            Type = ClientTypes.Public,
            ConsentType = ConsentTypes.Implicit,
            RedirectUris =
            {
                new Uri("com.yourapp.flutter://callback")
            },
            PostLogoutRedirectUris =
            {
                new Uri("com.yourapp.flutter://logout")
            },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                "scp:api"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });
    }
}
```

**ApplicationDbContext.cs:**

```csharp
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // OpenIddict tables will be created automatically
    }
}

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

**Authorization Controller (Login/Consent UI):**

```csharp
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorizationController(
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictAuthorizationManager authorizationManager,
        IOpenIddictScopeManager scopeManager,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _applicationManager = applicationManager;
        _authorizationManager = authorizationManager;
        _scopeManager = scopeManager;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    public async Task<IActionResult> Authorize()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve user from database
        var user = await _userManager.GetUserAsync(User) ??
            throw new InvalidOperationException("The user details cannot be retrieved.");

        // Create principal
        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        // Set requested scopes
        principal.SetScopes(request.GetScopes());
        principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

        // Create authorization
        var authorization = await _authorizationManager.CreateAsync(
            principal: principal,
            subject: await _userManager.GetUserIdAsync(user),
            client: await _applicationManager.GetIdAsync(
                await _applicationManager.FindByClientIdAsync(request.ClientId)),
            type: AuthorizationTypes.Permanent,
            scopes: principal.GetScopes());

        principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

        // Return authorization response
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            // Retrieve claims principal from auth code/refresh token
            var result = await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var user = await _userManager.FindByIdAsync(result.Principal.GetClaim(Claims.Subject));
            if (user == null)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = 
                            "The token is no longer valid."
                    }));
            }

            // Ensure user is still allowed to sign in
            if (!await _signInManager.CanSignInAsync(user))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = 
                            "The user is no longer allowed to sign in."
                    }));
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Return new access token
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("The specified grant type is not supported.");
    }
}
```

**Protected API Controller:**

```csharp
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(Claims.Subject)?.Value;
        var email = User.FindFirst(Claims.Email)?.Value;
        var name = User.FindFirst(Claims.Name)?.Value;

        return Ok(new
        {
            id = userId,
            email = email,
            name = name
        });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        // Admin only endpoint
        return Ok(new[] { "User1", "User2" });
    }
}
```

### 2. Angular 17 Client

**Install:**
```bash
npm install angular-oauth2-oidc
```

**auth.config.ts:**

```typescript
import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
  issuer: 'https://your-api.com', // Your OpenIddict server
  redirectUri: window.location.origin + '/callback',
  clientId: 'angular-spa',
  responseType: 'code',
  scope: 'openid profile email api',
  showDebugInformation: true,
  requireHttps: false, // Set to true in production
  useSilentRefresh: true,
  silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
};
```

**auth.service.ts:**

```typescript
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from './auth.config';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private oauthService: OAuthService,
    private router: Router
  ) {
    this.configure();
  }

  private configure() {
    this.oauthService.configure(authConfig);
    this.oauthService.setupAutomaticSilentRefresh();
    this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {
      if (this.oauthService.hasValidAccessToken()) {
        this.oauthService.loadUserProfile();
      }
    });
  }

  login() {
    this.oauthService.initCodeFlow();
  }

  logout() {
    this.oauthService.logOut();
  }

  getAccessToken(): string {
    return this.oauthService.getAccessToken();
  }

  get isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

  get userProfile() {
    return this.oauthService.getIdentityClaims();
  }
}
```

**HTTP Interceptor:**

```typescript
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const oauthService = inject(OAuthService);
  const token = oauthService.getAccessToken();

  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req);
};
```

### 3. Razor App (.NET Framework 4.7.2)

**Web.config:**
```xml
<configuration>
  <appSettings>
    <add key="oidc:Authority" value="https://your-api.com" />
    <add key="oidc:ClientId" value="razor-app" />
    <add key="oidc:ClientSecret" value="your-secret-here" />
    <add key="oidc:RedirectUri" value="https://your-razor-app.com/signin-oidc" />
  </appSettings>
</configuration>
```

**Install NuGet packages:**
```
Microsoft.Owin.Security.OpenIdConnect
Microsoft.Owin.Security.Cookies
Microsoft.Owin.Host.SystemWeb
```

**Startup.cs:**

```csharp
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.IdentityModel.Tokens;

[assembly: OwinStartup(typeof(YourApp.Startup))]

namespace YourApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["oidc:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["oidc:ClientSecret"],
                Authority = ConfigurationManager.AppSettings["oidc:Authority"],
                RedirectUri = ConfigurationManager.AppSettings["oidc:RedirectUri"],
                ResponseType = "code",
                Scope = "openid profile email api",
                SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                UseTokenLifetime = false,
                
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                }
            });
        }
    }
}
```

### 4. Flutter App

**pubspec.yaml:**
```yaml
dependencies:
  flutter_appauth: ^7.0.0
  flutter_secure_storage: ^9.0.0
```

**auth_service.dart:**

```dart
import 'package:flutter_appauth/flutter_appauth.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class AuthService {
  final FlutterAppAuth _appAuth = FlutterAppAuth();
  final FlutterSecureStorage _secureStorage = FlutterSecureStorage();

  static const String _clientId = 'flutter-app';
  static const String _redirectUri = 'com.yourapp.flutter://callback';
  static const String _issuer = 'https://your-api.com';
  static const String _discoveryUrl = 'https://your-api.com/.well-known/openid-configuration';

  Future<bool> login() async {
    try {
      final AuthorizationTokenResponse? result =
          await _appAuth.authorizeAndExchangeCode(
        AuthorizationTokenRequest(
          _clientId,
          _redirectUri,
          discoveryUrl: _discoveryUrl,
          scopes: ['openid', 'profile', 'email', 'api', 'offline_access'],
          promptValues: ['login'],
        ),
      );

      if (result != null) {
        await _secureStorage.write(key: 'access_token', value: result.accessToken);
        await _secureStorage.write(key: 'refresh_token', value: result.refreshToken);
        return true;
      }
      return false;
    } catch (e) {
      print('Error during login: $e');
      return false;
    }
  }

  Future<String?> getAccessToken() async {
    return await _secureStorage.read(key: 'access_token');
  }

  Future<void> logout() async {
    await _secureStorage.deleteAll();
  }

  Future<String?> refreshToken() async {
    try {
      final refreshToken = await _secureStorage.read(key: 'refresh_token');
      if (refreshToken == null) return null;

      final TokenResponse? result = await _appAuth.token(
        TokenRequest(
          _clientId,
          _redirectUri,
          discoveryUrl: _discoveryUrl,
          refreshToken: refreshToken,
        ),
      );

      if (result != null) {
        await _secureStorage.write(key: 'access_token', value: result.accessToken);
        await _secureStorage.write(key: 'refresh_token', value: result.refreshToken);
        return result.accessToken;
      }
    } catch (e) {
      print('Error refreshing token: $e');
    }
    return null;
  }
}
```

## Key Points

1. **OpenIddict server** handles all authentication and issues tokens
2. **Angular & Flutter** use PKCE (public clients, no secrets)
3. **Razor app** uses traditional Authorization Code flow (confidential client with secret)
4. **All clients** can call your protected API with access tokens
5. **ASP.NET Core Identity** manages users, roles, and claims

This setup gives you a complete, production-ready auth system!
