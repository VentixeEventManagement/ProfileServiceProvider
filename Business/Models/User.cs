using Microsoft.AspNetCore.Http;

namespace Business.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public IFormFile? ProfileImage { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set;}

}
