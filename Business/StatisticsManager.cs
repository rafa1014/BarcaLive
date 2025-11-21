// Author: Rafa och Rida
using System.Linq;
using System.Collections.Generic;

public class TeamStats
{
    public int Played { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public double GoalsPerMatch { get; set; }
    public string FormLast5 { get; set; } = string.Empty;
}

public class StatisticsManager
{
    public TeamStats CalculateStats(IEnumerable<MatchDto> matches, string team)
    {
        var fin = matches.Where(m => m.Status == "FINISHED").OrderByDescending(m => m.UtcDate).ToList();
        var s = new TeamStats();
        s.Played = fin.Count;
        s.Wins = fin.Count(m => IsWin(m, team));
        s.Draws = fin.Count(m => IsDraw(m));
        s.Losses = s.Played - s.Wins - s.Draws;
        s.GoalsPerMatch = fin.Any() ? fin.Sum(m => Goals(m, team)) / (double)fin.Count : 0;
        s.FormLast5 = string.Join(" ", fin.Take(5).Select(m => Letter(m, team)));
        return s;
    }
    bool IsWin(MatchDto m, string team) => m.HomeTeam == team ? (m.HomeScore ?? -1) > (m.AwayScore ?? -1) : (m.AwayScore ?? -1) > (m.HomeScore ?? -1);
    bool IsDraw(MatchDto m) => m.HomeScore.HasValue && m.AwayScore.HasValue && m.HomeScore == m.AwayScore;
    int Goals(MatchDto m, string team) => m.HomeTeam == team ? m.HomeScore ?? 0 : m.AwayScore ?? 0;
    string Letter(MatchDto m, string team) => IsWin(m, team) ? "W" : IsDraw(m) ? "D" : "L";
}
