namespace Domain.Models;

public class UserUpdateForm
{
    public string UserId { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
