# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

### Build
```bash
dotnet build Cirreum.Runtime.Client.Msal.slnx --configuration Release
```

### Restore dependencies
```bash
dotnet restore Cirreum.Runtime.Client.Msal.slnx
```

### Create NuGet package
```bash
dotnet pack Cirreum.Runtime.Client.Msal.slnx --configuration Release -p:Version=1.0.0
```

### Run a specific test
The project is a library without tests. Testing should be done in consumer applications.

## Architecture

This is a Blazor WebAssembly authentication library that integrates Microsoft Authentication Library (MSAL) with the Cirreum Framework. It provides:

### Key Components

1. **Authentication Builders** - Fluent API for configuring Azure Entra authentication
   - `IEntraAuthenticationBuilder` - For workforce tenants
   - `IEntraExternalBuilder` - For external identity tenants

2. **Graph Integration** - Microsoft Graph API services
   - User profile enrichment (minimal, common, extended)
   - Presence tracking
   - Custom claims transformation

3. **Extension Pattern** - Uses `HostingExtensions` to extend `IClientDomainApplicationBuilder`
   - `AddEntraAuth()` - Workforce authentication
   - `AddEntraExternalAuth()` - External tenant authentication

### Authentication Flow

1. MSAL authentication configured via `AddMsalAuthentication()`
2. Custom `MsalClaimsPrincipalFactory` for claims processing
3. Optional Graph services for profile enrichment
4. Session monitoring and user presence tracking

### Dependencies

- Target: .NET 10.0
- Microsoft.Authentication.WebAssembly.Msal 10.0.0
- Cirreum.Graph.Provider 1.0.5
- Cirreum.Runtime.Client 1.0.1

### Package Structure

- Solution uses `.slnx` format (VS 2022+)
- MSBuild props in `/build` directory for consistent packaging
- GitHub Actions workflow for automated NuGet publishing