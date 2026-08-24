using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskMgmt.Data;
using TaskMgmt.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtkey = builder.Configuration["Jwt:Key"];
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4300", "https://ambitious-field-0d3c25a00.7.azurestaticapps.net")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtkey!)),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Auto-run migrations and warm up the connection pool on startup
if (Environment.GetEnvironmentVariable("RUN_MIGRATIONS") == "true")
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
        
        // Pre-warm the connection pool
        try
        {
            var canConnect = await db.Database.CanConnectAsync();
            if (canConnect)
                app.Logger.LogInformation("Database connection successful.");
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning($"Could not warm up connection pool: {ex.Message}");
        }
    }
}
else
{
    // Pre-warm connection pool even if we skip migrations
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        try
        {
            var canConnect = await db.Database.CanConnectAsync();
            if (canConnect)
                app.Logger.LogInformation("Database connection pool warmed up.");
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning($"Could not warm up connection pool: {ex.Message}");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapGet("/health", () => Results.Ok("OK"))
    .WithName("Health")
    .WithOpenApi();

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Task Management API");
    options.RoutePrefix = "swagger";
});

app.UseRouting();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
