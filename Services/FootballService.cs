using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class FootballService : IFootballService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public FootballService(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _apiKey = cfg["FootballApi:ApiKey"] ?? string.Empty;
        if(!string.IsNullOrEmpty(_apiKey)) _http.DefaultRequestHeaders.Add("X-Auth-Token", _apiKey);
    }

    public async Task<IEnumerable<MatchDto>> GetTeamMatchesAsync(int teamId, DateTime from, DateTime to)
    {
        var url = $"teams/{teamId}/matches?dateFrom={from:yyyy-MM-dd}&dateTo={to:yyyy-MM-dd}";
        using var res = await _http.GetAsync(url);
        res.EnsureSuccessStatusCode();
        using var stream = await res.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);

        var list = new List<MatchDto>();
        if(doc.RootElement.TryGetProperty("matches", out var matchesEl)){
            foreach(var m in matchesEl.EnumerateArray()){
                var score = m.GetProperty("score").GetProperty("fullTime");
                list.Add(new MatchDto {
                    MatchId = m.GetProperty("id").GetInt32(),
                    UtcDate = m.GetProperty("utcDate").GetDateTime(),
                    Status = m.GetProperty("status").GetString() ?? string.Empty,
                    Competition = m.GetProperty("competition").GetProperty("name").GetString() ?? string.Empty,
                    HomeTeam = m.GetProperty("homeTeam").GetProperty("name").GetString() ?? string.Empty,
                    AwayTeam = m.GetProperty("awayTeam").GetProperty("name").GetString() ?? string.Empty,
                    HomeScore = score.GetProperty("home").ValueKind == JsonValueKind.Null ? (int?)null : score.GetProperty("home").GetInt32(),
                    AwayScore = score.GetProperty("away").ValueKind == JsonValueKind.Null ? (int?)null : score.GetProperty("away").GetInt32()
                });
            }
        }
        return list;
    }
}
