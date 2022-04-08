using Demo.ResourceServer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication(opt =>
    {
        opt.Authority = "http://localhost:5999";
        opt.RequireHttpsMetadata = false;
        opt.ApiName = "ApiResources";
    });
builder.Services.AddDbContext<BankContext>(options => options.UseInMemoryDatabase("BankingDb"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
