using Business.Models;
using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class UserFactory
{
    public static UserEntity Create(UserRegistrationForm formData)
    {
        var entity = new UserEntity
        {
            UserId = formData.UserId,
            ProfileImageUrl = formData.ProfileImageUrl,
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
            ProfileImageUrl = userEntity.ProfileImageUrl,
        };

        return user;
    }

    public static UserEntity Create(User user)
    {
        var entity = new UserEntity
        {
            UserId = user.UserId,
            ProfileImageUrl = user.ProfileImageUrl,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };

        return entity;
    }
}
