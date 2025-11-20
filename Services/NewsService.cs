// Author: Rida
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class NewsService : INewsService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public NewsService(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _apiKey = cfg["NewsApi:ApiKey"] ?? string.Empty;
    }

    public async Task<IEnumerable<NewsArticle>> GetNewsAsync(string query, int pageSize)
    {
        // Query NewsAPI for articles about query (filter to Barcelona related). Use 'q=' and language 'en' by default.
        var url = $"everything?q={Uri.EscapeDataString(query)}&pageSize={pageSize}&sortBy=publishedAt&language=en";
        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        if(!string.IsNullOrEmpty(_apiKey)) req.Headers.Add("X-Api-Key", _apiKey);
        using var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();
        using var stream = await res.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var list = new List<NewsArticle>();
        if(doc.RootElement.TryGetProperty("articles", out var artEl)){
            foreach(var a in artEl.EnumerateArray()){
                list.Add(new NewsArticle{
                    Title = a.GetProperty("title").GetString() ?? string.Empty,
                    Description = a.TryGetProperty("description", out var d) ? d.GetString() ?? string.Empty : string.Empty,
                    Url = a.GetProperty("url").GetString() ?? string.Empty,
                    Source = a.GetProperty("source").GetProperty("name").GetString() ?? string.Empty,
                    ImageUrl = a.TryGetProperty("urlToImage", out var img) && img.ValueKind != JsonValueKind.Null ? img.GetString() ?? string.Empty : string.Empty,
                    PublishedAt = a.GetProperty("publishedAt").GetDateTime()
                });
            }
        }
        return list;
    }
}
