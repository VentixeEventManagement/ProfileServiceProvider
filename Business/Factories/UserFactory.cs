using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserEntity Create(UserRegistrationForm formData, string? imageUri = null)
    {
        var entity = new UserEntity
        {
            UserId = formData.UserId,
            ProfileImageUrl = imageUri,
            FirstName = formData.FirstName,
            LastName = formData.LastName,
            PhoneNumber = formData.PhoneNumber,
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
            PhoneNumber= userEntity.PhoneNumber,
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
            PhoneNumber = user.PhoneNumber  
        };

        return entity;
    }
}
