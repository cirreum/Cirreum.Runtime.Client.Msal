namespace Cirreum.Runtime;

using Cirreum.Authorization;
using Cirreum.Runtime.Authentication;
using Cirreum.Runtime.Authentication.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using Microsoft.Extensions.DependencyInjection;

public static class HostingExtensions {

	internal static readonly HashSet<string> DefaultEntraScopes = [
		"openid",
		"profile",
		"email",
		"offline_access",
		"User.Read",
	];

	//
	// EntraId - Workforce
	//

	/// <summary>
	/// Configures Azure Entra Workforce authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="tenantId">The tenant ID. Use "common" for multi-tenant applications.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <returns>An <see cref="IEntraAuthenticationBuilder"/> for optionally adding Graph services.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra Workforce</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraAuthenticationBuilder AddEntraAuth(this IClientDomainApplicationBuilder builder,
		string tenantId,
		string clientId)
		=> builder.AddEntraAuth(tenantId, clientId, [], null);

	/// <summary>
	/// Configures Azure Entra Workforce authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="tenantId">The tenant ID. Use "common" for multi-tenant applications.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <returns>An <see cref="IEntraAuthenticationBuilder"/> for optionally adding Graph services.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra Workforce</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraAuthenticationBuilder AddEntraAuth(this IClientDomainApplicationBuilder builder,
		string tenantId,
		string clientId,
		Action<AuthorizationOptions> authorization)
		=> builder.AddEntraAuth(tenantId, clientId, [], authorization);

	/// <summary>
	/// Configures Azure Entra Workforce authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="tenantId">The tenant ID. Use "common" for multi-tenant applications.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <param name="defaultScopes">Optional. Custom OAuth scopes. If not provided, uses openid, profile, email, offline_access, and User.Read.</param>
	/// <returns>An <see cref="IEntraAuthenticationBuilder"/> for optionally adding Graph services.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra Workforce</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraAuthenticationBuilder AddEntraAuth(this IClientDomainApplicationBuilder builder,
		string tenantId,
		string clientId,
		params HashSet<string> defaultScopes)
			=> builder.AddEntraAuth(tenantId, clientId, defaultScopes, null);

	/// <summary>
	/// Configures Azure Entra Workforce authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="tenantId">The tenant ID. Use "common" for multi-tenant applications.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <param name="defaultScopes">Optional. Custom OAuth scopes. If not provided, uses openid, profile, email, offline_access, and User.Read.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <returns>An <see cref="IEntraAuthenticationBuilder"/> for optionally adding Graph services.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra Workforce</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraAuthenticationBuilder AddEntraAuth(this IClientDomainApplicationBuilder builder,
		string tenantId,
		string clientId,
		HashSet<string> defaultScopes,
		Action<AuthorizationOptions>? authorization = null) {

		// Scopes
		RegisterScopes(builder.Services, defaultScopes);

		// Entra ID
		RegisterMsal(builder.Services, clientId, $"https://login.microsoftonline.com/{tenantId}");

		// Authorization
		builder.AddDefaultAuthorization(authorization);

		// Graph Enablement
		return new EntraAuthenticationBuilder(builder.Services);

	}



	//
	// EntraId - External 
	//

	/// <summary>
	/// Configures Azure Entra External authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="domain">The external tenant's domain name.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <returns>An <see cref="IEntraExternalBuilder"/> for configuring additional Graph services for external tenants.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra External</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraExternalBuilder AddEntraExternalAuth(this IClientDomainApplicationBuilder builder,
		string domain,
		string clientId)
			=> builder.AddEntraExternalAuth(domain, clientId, [], null);

	/// <summary>
	/// Configures Azure Entra External authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="domain">The external tenant's domain name.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <param name="authorization">
	/// Optional callback to add additional policies. 
	/// Default policies already included are: 
	/// <see cref="AuthorizationPolicies.Standard"/>, 
	/// <see cref="AuthorizationPolicies.StandardInternal"/>,
	/// <see cref="AuthorizationPolicies.StandardAgent"/>, 
	/// <see cref="AuthorizationPolicies.StandardManager"/> and 
	/// <see cref="AuthorizationPolicies.StandardAdmin"/></param>
	/// <returns>An <see cref="IEntraExternalBuilder"/> for configuring additional Graph services for external tenants.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra External</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraExternalBuilder AddEntraExternalAuth(this IClientDomainApplicationBuilder builder,
		string domain,
		string clientId,
		Action<AuthorizationOptions> authorization)
			=> builder.AddEntraExternalAuth(domain, clientId, [], authorization);

	/// <summary>
	/// Configures Azure Entra External authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="domain">The external tenant's domain name.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <param name="defaultScopes">Optional. Custom OAuth scopes. If not provided, uses openid, profile, email, offline_access, and User.Read.</param>
	/// <returns>An <see cref="IEntraExternalBuilder"/> for configuring additional Graph services for external tenants.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra External</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraExternalBuilder AddEntraExternalAuth(this IClientDomainApplicationBuilder builder,
		string domain,
		string clientId,
		params HashSet<string> defaultScopes)
			=> builder.AddEntraExternalAuth(domain, clientId, defaultScopes, null);

	/// <summary>
	/// Configures Azure Entra External authentication for a Blazor WebAssembly application.
	/// </summary>
	/// <param name="builder">The <see cref="IClientDomainApplicationBuilder"/>.</param>
	/// <param name="domain">The external tenant's domain name.</param>
	/// <param name="clientId">The Azure application (client) ID.</param>
	/// <param name="defaultScopes">Optional. Custom OAuth scopes. If not provided, uses openid, profile, email, offline_access, and User.Read.</param>
	/// <param name="authorization">Optional. Callback to configure additional authorization policies beyond the default app role policies.</param>
	/// <returns>An <see cref="IEntraExternalBuilder"/> for configuring additional Graph services for external tenants.</returns>
	/// <remarks>
	/// <para>
	/// This method sets up:
	/// <list type="bullet">
	///     <item>
	///         <description>MSAL authentication with Azure Entra External</description>
	///     </item>
	///     <item>
	///         <description>Default authorization policies</description>
	///     </item>
	///     <item>
	///         <description>Default roles</description>
	///     </item>
	/// </list>
	/// </para>
	/// </remarks>
	public static IEntraExternalBuilder AddEntraExternalAuth(this IClientDomainApplicationBuilder builder,
		string domain,
		string clientId,
		HashSet<string> defaultScopes,
		Action<AuthorizationOptions>? authorization = null) {

		// Scopes
		RegisterScopes(builder.Services, defaultScopes);

		// Entra External ID
		RegisterMsal(builder.Services, clientId, $"https://{domain}.ciamlogin.com/", false);

		// Authorization
		builder.AddDefaultAuthorization(authorization);

		// External Graph Enablement
		return new EntraExternalBuilder(new EntraAuthenticationBuilder(builder.Services));

	}



	//
	// Msal Authentication
	//

	private static void RegisterMsal(IServiceCollection services, string clientId, string authority, bool validateAuthority = true) {
		const string loginMode = "redirect";
		services
			.AddMsalAuthentication<RemoteAuthenticationState, EntraUserAccount>(o => {
				o.UserOptions.NameClaim = "name";
				o.UserOptions.RoleClaim = "roles";
				o.ProviderOptions.LoginMode = loginMode;
				o.ProviderOptions.Authentication.ClientId = clientId;
				o.ProviderOptions.Authentication.Authority = authority;
				o.ProviderOptions.Authentication.ValidateAuthority = validateAuthority;
			})
			.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, EntraUserAccount, MsalClaimsPrincipalFactory>();
	}

	private static void RegisterScopes(IServiceCollection services, HashSet<string> defaultScopes) {
		var localDefaultScopes = DefaultEntraScopes;
		if (defaultScopes.Count > 0) {
			localDefaultScopes.UnionWith(defaultScopes);
		}
		services.Configure<RemoteAuthenticationOptions<MsalProviderOptions>>(options => {
			localDefaultScopes.ToList().ForEach(options.ProviderOptions.DefaultAccessTokenScopes.Add);
		});
	}

}