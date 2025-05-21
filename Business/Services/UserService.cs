using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ResponseResult> AddUserInfoasync(UserRegistrationForm form)
    {
        try
        {
            var entity = UserFactory.Create(form);
            if (entity == null)
                return new ResponseResult { Succeeded = false, Message = "Invalid registration form.", StatusCode = 422 };

            var result = await _userRepository.AddAsync(entity);
            if (!result)
                return new ResponseResult { Succeeded = false, Message = "Something went wrong with creation.", StatusCode = 500 };

            return new ResponseResult { Succeeded = true, Message = "Profile information was created successfully.", StatusCode = 201 };

        }
        catch (Exception ex)
        {
            return new ResponseResult { Succeeded = false, Message = ex.Message, StatusCode = 500 };
        }
    }

    public async Task<ResponseResult<User>> GetUserInfoAsync(Expression<Func<UserEntity, bool>> expression)
    {
        var entity = await _userRepository.GetAsync(expression);
        if (entity == null)
            return new ResponseResult<User> { Succeeded = false, Message = "User was not found.", StatusCode = 404 };

        var user = UserFactory.Create(entity);
        if (user == null)
            return new ResponseResult<User> { Succeeded = false, Message = "Failed to map user.", StatusCode = 500 };

        return new ResponseResult<User> { Succeeded = true, StatusCode = 200, Result = user };
    }

    public async Task<ResponseResult> UpdateProfileInfoAsync(string userId, UserUpdateForm user)
    {
        try
        {
            if (user == null)
                return new ResponseResult { Succeeded = false, Message = "User is null", StatusCode = 400 };

            //var entity = UserFactory.Create(user, userId);
            //if (entity == null)
            //    return new ResponseResult { Succeeded = false, Message = "Invalid user information", StatusCode = 422 };

            var updated = await _userRepository.UpdateAsync(userId,  user);
            if (!updated)
                return new ResponseResult<User> { Succeeded = false, Message = "Couldn't update user information.", StatusCode = 500, };

            return new ResponseResult<User> { Succeeded = true, Message = "User information updated successfully.", StatusCode = 200 };
        }
        catch (Exception ex)
        {
            return new ResponseResult { Succeeded = false, Message = ex.Message, StatusCode = 500 };
        }
    }

    public async Task<ResponseResult> DeleteProfileInfoAsync(User user)
    {
        try
        {
            if (user == null)
                return new ResponseResult { Succeeded = false, Message = "User is null", StatusCode = 400 };

            var entity = UserFactory.Create(user);
            if (entity == null)
                return new ResponseResult { Succeeded = false, Message = "Invalid user information", StatusCode = 422 };

            var deleted = await _userRepository.DeleteAsync(entity);
            if (!deleted)
                return new ResponseResult<User> { Succeeded = false, Message = "Couldn't delete user information.", StatusCode = 500, };

            return new ResponseResult<User> { Succeeded = true, Message = "User information deleted successfully.", StatusCode = 200 };
        }
        catch (Exception ex)
        {
            return new ResponseResult { Succeeded = false, Message = ex.Message, StatusCode = 500 };
        }
    }
}
