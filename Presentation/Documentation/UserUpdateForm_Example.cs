using Domain.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace Presentation.Documentation;

public class UserUpdateForm_Example : IExamplesProvider<UserUpdateForm>
{
    public UserUpdateForm GetExamples()
    {
        // Mock data
        var content = Encoding.UTF8.GetBytes("<svg></svg>");
        var stream = new MemoryStream(content);

        var formFile = new FormFile(stream, 0, content.Length, "ProfileImageUri", "Avater.svg")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/svg+xml"
        };

        return new UserUpdateForm
        {
            UserId = "12345678-abcd-1234-abcd-1234567890ab",
            ProfileImageUri = formFile,
            FirstName = "John",
            LastName = "Doe",
        };
    }

}
