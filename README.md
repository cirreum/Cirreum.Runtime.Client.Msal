# Cirreum.Runtime.Client.Msal

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Runtime.Client.Msal.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Runtime.Client.Msal/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.Runtime.Client.Msal.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Runtime.Client.Msal/)
[![GitHub Release](https://img.shields.io/github/v/release/cirreum/Cirreum.Runtime.Client.Msal?style=flat-square&labelColor=1F1F1F&color=FF3B2E)](https://github.com/cirreum/Cirreum.Runtime.Client.Msal/releases)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.Runtime.Client.Msal?style=flat-square&labelColor=1F1F1F&color=F2F2F2)](https://github.com/cirreum/Cirreum.Runtime.Client.Msal/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Seamless Azure Entra authentication for Blazor WebAssembly applications**

## Overview

**Cirreum.Runtime.Client.Msal** provides a fluent API for integrating Microsoft Authentication Library (MSAL) with Blazor WebAssembly applications in the Cirreum Framework. It simplifies Azure Entra ID (formerly Azure AD) authentication setup for both workforce and external identity scenarios.

## Features

- **Workforce & External Identity Support** - Configure authentication for both internal users and external customers
- **Microsoft Graph Integration** - Built-in support for user profile enrichment and presence tracking
- **Fluent Configuration API** - Chainable methods for intuitive authentication setup
- **Session Monitoring** - Track user activity and API calls
- **Claims Extension** - Customize user claims with your own transformations
- **Authorization Policies** - Pre-configured standard policies with extensibility

## Installation

```bash
dotnet add package Cirreum.Runtime.Client.Msal
```

## Usage

### Basic Workforce Authentication

```csharp
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.AddCirreumClient()
    .AddEntraAuth(
        tenantId: "your-tenant-id",  // or "common" for multi-tenant
        clientId: "your-client-id"
    );

await builder.Build().RunAsync();
```

### External Identity Authentication

```csharp
builder.AddCirreumClient()
    .AddEntraExternalAuth(
        domain: "your-domain",
        clientId: "your-client-id"
    );
```

### With Microsoft Graph Services

```csharp
builder.AddCirreumClient()
    .AddEntraAuth("your-tenant-id", "your-client-id")
    .AddGraphServices()
    .AddMinimalUserProfile()    // or AddCommonUserProfile() or AddExtendedUserProfile()
    .AddPresenceTracking();
```

### Custom Authorization Policies

```csharp
builder.AddCirreumClient()
    .AddEntraAuth("your-tenant-id", "your-client-id", authorization: options =>
    {
        options.AddPolicy("CustomPolicy", policy =>
            policy.RequireClaim("department", "Engineering"));
    });
```

### Session Monitoring

```csharp
builder.AddCirreumClient()
    .AddEntraAuth("your-tenant-id", "your-client-id")
    .AddSessionMonitoring(options =>
    {
        options.TrackApiCalls = true;
        options.IdleTimeout = TimeSpan.FromMinutes(30);
    });
```

## Contribution Guidelines

1. **Be conservative with new abstractions**  
   The API surface must remain stable and meaningful.

2. **Limit dependency expansion**  
   Only add foundational, version-stable dependencies.

3. **Favor additive, non-breaking changes**  
   Breaking changes ripple through the entire ecosystem.

4. **Include thorough unit tests**  
   All primitives and patterns should be independently testable.

5. **Document architectural decisions**  
   Context and reasoning should be clear for future maintainers.

6. **Follow .NET conventions**  
   Use established patterns from Microsoft.Extensions.* libraries.

## Versioning

Cirreum.Runtime.Client.Msal follows [Semantic Versioning](https://semver.org/):

- **Major** - Breaking API changes
- **Minor** - New features, backward compatible
- **Patch** - Bug fixes, backward compatible

Given its foundational role, major version bumps are rare and carefully considered.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Cirreum Foundation Framework**  
*Layered simplicity for modern .NET*