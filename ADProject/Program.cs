using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ADProject.Services;
using ADProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ---- CORS��ֻ����ǰ��ҳ��� Origin��������ƾ�� ----
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000"    // ���� React
                                           // ���������ǰ����ʽ����Ҳ�ӽ��������� "https://www.example.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();          // ���룺����Я�� Cookie
    });
});

// ---- Session����վ Cookie ��Ҫ SameSite=None���������Ȳ�ǿ�� Secure ----
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.Name = ".adproject.sid";
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SameSite = SameSiteMode.None;            // ��վ���� None
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // ����/HTTP �� None������ HTTPS �� Always
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

// ---- �м��ִ��˳��ܹؼ� ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ACI/�����õ��� HTTP���ȹص�ǿ�� HTTPS������� 307 �� https ����ʧ�ܣ�
// �����ȷʵ����֤���ٴ򿪡�
// app.UseHttpsRedirection();

app.UseCors("Frontend");   //CORS Ҫ��ǰ
app.UseSession();          //ʹ�� Session ��������
app.UseAuthorization();

app.MapControllers();

// ��ѡ������� EnsureCreated ��Ŀ�ṹ������������
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

