using CafeBooking.Web.Data;
using CafeBooking.Web.Models;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
    options.SamplingRatio = 1.0f; // Capture 100% of telemetry for demo purposes
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "text/html",
        "text/css",
        "application/javascript"
    });
});


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.SectionName));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromMinutes(5)));
    options.AddPolicy("MenuCache", builder => builder.Expire(TimeSpan.FromMinutes(10)));
});

var app = builder.Build();

app.UseOutputCache();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 year
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// DEMO ONLY — remove after recording
app.MapGet("/config-check", (IConfiguration config) =>
{
    var cs = config.GetConnectionString("DefaultConnection");
    return cs?.Contains("database.windows.net") == true
        ? "AZURE => Using Azure SQL"
        : "LOCAL => Using LocalDB";
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = new DbSeeder(context);

    if (app.Environment.IsDevelopment())
    {
        await context.Database.MigrateAsync(); // Auto-apply migrations
        await seeder.SeedAsync();
    }
    else
    {
        await seeder.SeedAsync(); // Production: migrations applied in CI/CD
    }
}

app.Run();
