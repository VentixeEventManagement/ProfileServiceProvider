using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class UserUpdateForm
{
    public string UserId { get; set; } = null!;
    public IFormFile? ProfileImageUri { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
