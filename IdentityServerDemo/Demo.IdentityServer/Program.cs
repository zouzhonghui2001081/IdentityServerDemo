using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false); 
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false)
    .Build();

var conString = config.GetConnectionString("identityserver4db");
var migrationAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddTestUsers(Config.GetTestUsers())
    .AddConfigurationStore(options =>
    {
        // stores clients and resources
        options.ConfigureDbContext = context => context.UseNpgsql(conString, pgsql => pgsql.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(options =>
    {
        // stores tokens, consents, codes
        options.ConfigureDbContext = context => context.UseNpgsql(conString, pgsql => pgsql.MigrationsAssembly(migrationAssembly));
    });
//.AddInMemoryIdentityResources(Config.GetIdentityResources())
//.AddInMemoryApiResources(Config.GetApiResources())
//.AddInMemoryApiScopes(Config.GetApiScopes())
//.AddInMemoryClients(Config.GetClients())
//.AddTestUsers(Config.GetTestUsers());

// fixing The cookie 'idsrv' has set 'SameSite=None' and must also set 'Secure'. issue
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});

var app = builder.Build();

InitializeIdentityServer4DB(app);

// fixing The cookie 'idsrv' has set 'SameSite=None' and must also set 'Secure'. issue
app.UseCookiePolicy();
app.UseIdentityServer();
app.UseStaticFiles();
app.UseMvcWithDefaultRoute();

app.Run();



void InitializeIdentityServer4DB(WebApplication app)
{
    using(var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var persistedDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        persistedDbContext.Database.Migrate();

        var configDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        configDbContext.Database.Migrate();

        // Seed the data
        if(!configDbContext.Clients.Any())
        {
            foreach(var client in Config.GetClients())
            {
                configDbContext.Clients.Add(client.ToEntity());
            }

            configDbContext.SaveChanges();
        }
        if(!configDbContext.IdentityResources.Any())
        {
            foreach (var resource in Config.GetIdentityResources())
            {
                configDbContext.IdentityResources.Add(resource.ToEntity());
            }
            configDbContext.SaveChanges();
        }
        if(!configDbContext.ApiResources.Any())
        {
            foreach(var apiResource in Config.GetApiResources())
            {
                configDbContext.ApiResources.Add(apiResource.ToEntity());
            }
            configDbContext.SaveChanges();
        }

    }
}
