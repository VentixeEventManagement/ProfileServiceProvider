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

    public static User Create(UserEntity userEntity)
    {
        var user = new User
        {
            Id = userEntity.Id,
            UserId = userEntity.UserId,
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName,
            ProfileImage = userEntity.ProfileImage,
        };

        return user;
    }
}
