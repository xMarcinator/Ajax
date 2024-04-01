using backend.DB;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PersonContext>();

#if DEBUG
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
    });
}
#endif

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = serviceScope.ServiceProvider.GetRequiredService<PersonContext>().Database;

    logger.LogInformation("Migrating database...");
    var retries = 10;
    var delayMs = 1000;

    while (!db.CanConnect() && retries-- > 0)
    {
        logger.LogInformation($"Database not ready yet; waiting... (Attempts left: {retries})");
        Thread.Sleep(delayMs);
    }

    try
    {
        serviceScope.ServiceProvider.GetRequiredService<PersonContext>().Database.Migrate();
        logger.LogInformation("Database migrated successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"An error occurred while migrating the database after {10 - retries} retries.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

#if DEBUG
app.UseCors(MyAllowSpecificOrigins);
#endif

app.MapControllers();

app.MapControllerRoute(name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();