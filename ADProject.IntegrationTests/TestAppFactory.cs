using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ADProject.Services;

namespace ADProject.IntegrationTests;

// TODO: 把 AppDbContext 换成你项目里的 DbContext 实际类型
public class TestAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test"); // 标记为测试环境

        builder.ConfigureServices(services =>
        {
            // 1) 移除生产环境注册的 DbContext（MySQL）
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor is not null) services.Remove(descriptor);

            // 2) 改为使用 InMemory 数据库
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            // 3) 构建一个作用域，确保 InMemory DB 创建好（可在此处 Seed 数据）
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
