namespace WebApplication1.DTO;

public class StudentCreateDTO
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
        
    public int? AddressId { get; set; }
    public string? City { get; set; }
}