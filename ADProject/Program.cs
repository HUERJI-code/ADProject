using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ADProject.Services;
using ADProject.Models;
using ADProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<ActivityRepository>();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
initDB();
app.Run();

void initDB()
{
    // create the environment to retrieve our database context
    using (var scope = app.Services.CreateScope())
    {
        // get database context from DI-container
        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (!ctx.Database.CanConnect())
            ctx.Database.EnsureCreated(); // create database
    }
}
