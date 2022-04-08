using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

internal class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>()
        {
            new ApiResource("backendApiResource", "Resource Server Backend")
            {
                Scopes = new List<string>{ "resourceServerScope1", "resourceServerScope2" }
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new[]
        {
            new ApiScope(name: "resourceServerScope1",   displayName: "Backend Api Scope1"),
            new ApiScope(name: "resourceServerScope2",   displayName: "Backend Api Scope2")
        };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            // client credential grant type client
            new Client
            {
                ClientId = "client.clientcredential",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedScopes = { "resourceServerScope1", "resourceServerScope2" }
            },
        };
    }
}