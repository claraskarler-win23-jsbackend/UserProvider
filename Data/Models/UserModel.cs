namespace Data.Models;

public class UserModel
{

    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Biography { get; set; }
    public string Email { get; set; } = null!;
    public object? PhoneNumber { get; set; }
}
