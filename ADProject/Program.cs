using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ADProject.Services;
using ADProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ---- CORS：只允许前端页面的 Origin，并允许凭据 ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000"    // 本地 React
                                           // 将来把你的前端正式域名也加进来，比如 "https://www.example.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();          // 必须：允许携带 Cookie
    });
});

// ---- Session：跨站 Cookie 需要 SameSite=None，开发期先不强制 Secure ----
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.Name = ".adproject.sid";
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SameSite = SameSiteMode.None;            // 跨站必须 None
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // 开发/HTTP 用 None；上线 HTTPS 改 Always
});

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler =
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<ActivityRepository>();
builder.Services.AddScoped<ChannelRepository>();
builder.Services.AddScoped<SystemMessageRepository>();
builder.Services.AddHttpClient();

var app = builder.Build();

// ---- 中间件执行顺序很关键 ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ACI/本地用的是 HTTP，先关掉强制 HTTPS（否则会 307 跳 https 导致失败）
// 如果你确实配了证书再打开。
// app.UseHttpsRedirection();

app.UseCors("Frontend");   //CORS 要靠前
app.UseSession();          //使用 Session 必须启用
app.UseAuthorization();

app.MapControllers();

// 可选：你这个 EnsureCreated 会改库结构，生产不建议
if (!app.Environment.IsEnvironment("Test"))
{
    initDB();
}

app.Run();


void initDB()
{
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!ctx.Database.CanConnect())
        ctx.Database.EnsureCreated();
}

public partial class Program { }

