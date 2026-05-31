using System.Text.Json;
using SGEducationNigeriaMobile.Models;

namespace SGEducationNigeriaMobile.Services;

public class ArticleApiService
{
    private readonly HttpClient _http = new HttpClient
    {
        BaseAddress = new Uri("http://api.sgeducationnigerialtd.com") // change when using phone/emulator
    };

    public async Task<List<Article>> GetArticlesAsync()
    {
    
            var response = await _http.GetAsync("/api/article");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Article>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
     
    }
}
