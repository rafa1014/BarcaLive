// Author: Erfan
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class MatchesController : Controller
{
    private readonly IFootballService _football;
    public MatchesController(IFootballService football) => _football = football;

    public async Task<IActionResult> Index()
    {
        var from = System.DateTime.UtcNow.AddMonths(-6);
        var to = System.DateTime.UtcNow.AddMonths(6);
        var matches = await _football.GetTeamMatchesAsync(81, from, to);
        return View(matches);
    }
}
