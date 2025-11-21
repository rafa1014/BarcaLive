// Author: Rida och Rafa
using System.Collections.Generic;

public class FullDashboardViewModel
{
    public IEnumerable<MatchDto> Matches { get; set; } = new List<MatchDto>();
    public TeamStats Stats { get; set; } = new TeamStats();
    public IEnumerable<NewsArticle> News { get; set; } = new List<NewsArticle>();
}
