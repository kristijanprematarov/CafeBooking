using CafeBooking.Web.Data;
using CafeBooking.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection(AppSettings.SectionName));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

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
