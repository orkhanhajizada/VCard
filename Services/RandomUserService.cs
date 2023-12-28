namespace VCard.Services;

public class RandomUserService (int result)
{
    private readonly string _apiUrl = $"https://randomuser.me/api?results={result}";

    public async Task<string> GetRandomUserData()
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync(_apiUrl);
        return response;
    }
}