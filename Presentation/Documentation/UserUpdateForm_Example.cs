using Domain.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Documentation;

public class UserUpdateForm_Example : IExamplesProvider<UserUpdateForm>
{
    public UserUpdateForm GetExamples() => new()
    {
        UserId = "12345678-abcd-1234-abcd-1234567890ab",
        ProfileImageUrl = "Avatar.svg",
        FirstName = "John",
        LastName = "Doe",
    };

}
