// Author: Rafa och Rida 
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly IFootballService _football;
    private readonly INewsService _news;
    private readonly StatisticsManager _stats;
    private const int BarcelonaTeamId = 81;
    private const string BarcelonaName = "FC Barcelona";

    public HomeController(IFootballService football, INewsService news, StatisticsManager stats)
    {
        _football = football;
        _news = news;
        _stats = stats;
    }

    public async Task<IActionResult> Index()
    {
        var from = DateTime.UtcNow.AddMonths(-6);
        var to = DateTime.UtcNow.AddMonths(6);

        var matches = (await _football.GetTeamMatchesAsync(BarcelonaTeamId, from, to)).ToList();
        var news = (await _news.GetNewsAsync("FC Barcelona", 10)).ToList();

        var model = new FullDashboardViewModel
        {
            Matches = matches,
            Stats = _stats.CalculateStats(matches, BarcelonaName),
            News = news
        };

        return View(model);
    }
}
