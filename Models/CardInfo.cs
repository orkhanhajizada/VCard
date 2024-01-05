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
}