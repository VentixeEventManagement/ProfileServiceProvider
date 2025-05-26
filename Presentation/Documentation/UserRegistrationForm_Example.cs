using Business.Models;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;
using System.IO;
using System.Text;

namespace Presentation.Documentation;

public class UserRegistrationForm_Example : IExamplesProvider<UserRegistrationForm>
{
    public UserRegistrationForm GetExamples()
    {
        var content = "fake image content";
        var fileName = "avatar.png";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        IFormFile formFile = new FormFile(stream, 0, stream.Length, "ProfileImageUri", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        return new UserRegistrationForm
        {
            UserId = "12345678-abcd-1234-abcd-1234567890ab",
            FirstName = "John",
            LastName = "Doe",
            ProfileImageUri = formFile
        };
    }
}
