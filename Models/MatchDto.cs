// Author: Rida
using System;

public class MatchDto
{
    public int MatchId { get; set; }
    public DateTime UtcDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Competition { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
}
