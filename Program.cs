// Author: Erfan
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// register MVC
builder.Services.AddControllersWithViews();

// configure typed http clients and services
builder.Services.AddHttpClient<IFootballService, FootballService>(client => {
    client.BaseAddress = new Uri(builder.Configuration["FootballApi:BaseUrl"] ?? "https://api.football-data.org/v4/");
    client.DefaultRequestHeaders.Add("User-Agent", "BarcaLiveApp");
});
builder.Services.AddHttpClient<INewsService, NewsService>(client => {
    client.BaseAddress = new Uri(builder.Configuration["NewsApi:BaseUrl"] ?? "https://newsapi.org/v2/");
    client.DefaultRequestHeaders.Add("User-Agent", "BarcaLiveApp");
});

builder.Services.AddScoped<StatisticsManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
