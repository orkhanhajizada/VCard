namespace VCard.Models;

public class CardInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Firstname { get; set; } 
    public string? Surname { get; set; }
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public string? Country { get; set; }
    public string? City { get; set; } 
    
    // public static CardInfo FromJson(string jsonData,int index)
    // {
    //     var jsonObject = JsonDocument.Parse(jsonData).RootElement.GetProperty("results")[0];
    //
    //     return new CardInfo
    //     {
    //         Firstname = jsonObject.GetProperty("name").GetProperty("first").GetString(),
    //         Surname = jsonObject.GetProperty("name").GetProperty("last").GetString(),
    //         Email = jsonObject.GetProperty("email").GetString(),
    //         Phone = jsonObject.GetProperty("phone").GetString(),
    //         Country = jsonObject.GetProperty("location").GetProperty("country").GetString(),
    //         City = jsonObject.GetProperty("location").GetProperty("city").GetString()
    //     };
    // }
}