using IdentityModel.Client;

public static class HttpExtensions
{

    #region Client Credential Grant Type
    public static async Task HandleToken(this HttpClient client, string authority, string clientId, string secret, string scope)
    {
        var accessToken = await client.GetRefreshTokenAsync(authority, clientId, secret, scope);
        client.SetBearerToken(accessToken);
    }

    private static async Task<string> GetRefreshTokenAsync(this HttpClient client, string authority, string clientId, string secret, string scope)
    {
        var disco = await client.GetDiscoveryDocumentAsync(authority);
        if (disco.IsError) throw new Exception(disco.Error);

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = secret,
            Scope = scope
        });

        if (!tokenResponse.IsError)
            return tokenResponse.AccessToken;
        return null;
    }

    #endregion

    #region Resource Owner Password Grant Type

    public static async Task HandleToken(this HttpClient client, string authority, string clientId, string secret, string username, string password, string scope)
    {
        var accessToken = await client.GetRefreshTokenAsync(authority, clientId, secret, username, password, scope);
        client.SetBearerToken(accessToken);
    }

    private static async Task<string> GetRefreshTokenAsync(this HttpClient client, string authority, string clientId, string secret, string username, string password, string scope)
    {
        var disco = await client.GetDiscoveryDocumentAsync(authority);
        if (disco.IsError) throw new Exception(disco.Error);

        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = clientId,
            ClientSecret = secret,
            UserName = username,
            Password = password,
            Scope = scope
        });

        if (!tokenResponse.IsError)
            return tokenResponse.AccessToken;
        return null;
    }

    #endregion
}