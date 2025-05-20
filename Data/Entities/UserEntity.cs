using Microsoft.AspNetCore.Http;

namespace Data.Entities;

public class UserEntity
{
    public IFormFile? ProfileImage { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
