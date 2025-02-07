using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using HomeEnergyApi.Models;
using HomeEnergyApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HomeRepository>();
builder.Services.AddScoped<IReadRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());
builder.Services.AddScoped<IWriteRepository<int, Home>>(provider => provider.GetRequiredService<HomeRepository>());

builder.Services.AddTransient<ZipCodeLocationService>();
builder.Services.AddHttpClient<ZipCodeLocationService>();

builder.Services.AddDbContext<HomeDbContext>(options =>
    options.UseSqlite("Data Source=Homes.db"));

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HomeDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();

//Do NOT remove anything below this comment, this is required to autograde the lesson
public partial class Program { }