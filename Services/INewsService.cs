// Author: Rida och Rafa
using System.Collections.Generic;
using System.Threading.Tasks;

public interface INewsService
{
    Task<IEnumerable<NewsArticle>> GetNewsAsync(string query, int pageSize);
}
