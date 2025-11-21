// Author: Rafa och Rida
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class NewsController : Controller
{
    private readonly INewsService _news;
    public NewsController(INewsService news) => _news = news;

    public async Task<IActionResult> Index()
    {
        var list = await _news.GetNewsAsync("FC Barcelona", 20);
        return View(list);
    }
}
