using Business.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Documentation;

public class UserRegistrationForm_Example : IExamplesProvider<UserRegistrationForm>
{
    public UserRegistrationForm GetExamples() => new()
    {
        UserId = "12345678-abcd-1234-abcd-1234567890ab",
        ProfileImageUrl = "Avatar.svg",
        FirstName = "John",
        LastName = "Doe",
    };
}
