using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserEntity Create(UserRegistrationForm formData)
    {
        var entity = new UserEntity
        {
            UserId = formData.UserId,
            ProfileImage = formData.ProfileImage,
            FirstName = formData.FirstName,
            LastName = formData.LastName,
        };

        return entity;
    }
}
