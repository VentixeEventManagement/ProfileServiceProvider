using Microsoft.AspNetCore.Http;

namespace Business.Models;

public class UserRegistrationForm
{
    public string UserId { get; set; } = null!;
    public IFormFile? ProfileImageUri { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}
