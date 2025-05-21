namespace Business.Models;

public class UserRegistrationForm
{
    public string UserId { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}
