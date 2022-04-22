var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false); 
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiResources(Config.GetApiResources())
    .AddInMemoryApiScopes(Config.GetApiScopes())
    .AddInMemoryClients(Config.GetClients())
    .AddTestUsers(Config.GetTestUsers());

// fixing The cookie 'idsrv' has set 'SameSite=None' and must also set 'Secure'. issue
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

// fixing The cookie 'idsrv' has set 'SameSite=None' and must also set 'Secure'. issue
app.UseCookiePolicy();
app.UseIdentityServer();
app.UseStaticFiles();
app.UseMvcWithDefaultRoute();

app.Run();
