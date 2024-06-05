using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Services;

namespace TaskManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        ConfigureDatabase(builder.Services, builder.Configuration);

        ConfigureServices(builder.Services);

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

        app.MapIdentityApi<User>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<IProjectTaskService, ProjectTaskService>();
    }
}
