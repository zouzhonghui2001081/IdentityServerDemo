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
            // resource owner password grant type client
            new Client
            {
                ClientId = "client.resourceowner",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedScopes = { "resourceServerScope1", "resourceServerScope2" }
            },
            // implicit grant type client
            new Client
            {
                ClientId = "client.implicit",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                RedirectUris = { "http://localhost:5555/signin-oidc" }, // !!!! mvc client uri and port
                PostLogoutRedirectUris = { "http://localhost:555/signout-callback-oidc" }, // !!!! mvc client uri and port
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            },
        };
    }

    public static List<TestUser> GetTestUsers()
    {
        return new List<TestUser>
        {
            new TestUser{ SubjectId = "1", Username="ReourceOwner1", Password="Admin123"},
            new TestUser{ SubjectId = "2", Username="ReourceOwner2", Password="Admin123"},
        };
    }
}