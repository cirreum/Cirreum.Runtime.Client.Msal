namespace Cirreum.Runtime.Authentication.Providers;

using Cirreum.Graph.Provider;
using Cirreum.Security;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System.Collections.Concurrent;

sealed class DefaultGraphServiceClientProvider : IGraphServiceClientProvider {

	internal static readonly string HttpClientName = "_GraphHttpClient";
	internal static readonly string GraphUrl = "https://graph.microsoft.com/v1.0";

	private readonly IServiceProvider _serviceProvider;
	private readonly List<string> _scopes;
	private readonly HttpClient _httpClient;
	private readonly ConcurrentDictionary<string, (GraphServiceClient client, IServiceScope scope, DateTime created)> _clientCache = new();
	private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(1);

	public DefaultGraphServiceClientProvider(
		IServiceProvider serviceProvider) {

		this._serviceProvider = serviceProvider;
		this._scopes = serviceProvider.GetService<GraphAuthenticationOptions>()?.RequiredScopes ??
			GraphEnabledBuilderExtensions.MinimalGraphScopes;

		// Reuse the HttpClient
		var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
		this._httpClient = httpClientFactory?.CreateClient(HttpClientName) ?? new HttpClient();
	}

	public async Task<T> UseClientAsync<T>(Func<GraphServiceClient, Task<T>> action)
		=> await action(this.GetClient());

	public async Task UseClientAsync(Func<GraphServiceClient, Task> action)
		=> await action(this.GetClient());

	private GraphServiceClient GetClient() {

		var user = this._serviceProvider.GetService<IUserState>();
		var cacheKey = user?.Name ?? "anonymous";

		// Clean up expired entries
		this.CleanupExpiredEntries();

		// Return cached client if still valid
		if (this._clientCache.TryGetValue(cacheKey, out var cached) &&
			DateTime.UtcNow - cached.created < CacheExpiry) {
			return cached.client;
		}

		// Create new client with scope
		var scope = this._serviceProvider.CreateScope();
		var tokenProvider = scope.ServiceProvider.GetRequiredService<IAccessTokenProvider>();
		var authProvider = new GraphAuthenticationProvider(tokenProvider, this._scopes);
		var client = new GraphServiceClient(this._httpClient, authProvider, GraphUrl);

		this._clientCache[cacheKey] = (client, scope, DateTime.UtcNow);
		return client;
	}

	private void CleanupExpiredEntries() {
		var keysToRemove = this._clientCache
			.Where(kvp => DateTime.UtcNow - kvp.Value.created >= CacheExpiry)
			.Select(kvp => kvp.Key)
			.ToList();

		foreach (var key in keysToRemove) {
			if (this._clientCache.TryRemove(key, out var cached)) {
				cached.client?.Dispose();
				cached.scope?.Dispose();
			}
		}
	}

}