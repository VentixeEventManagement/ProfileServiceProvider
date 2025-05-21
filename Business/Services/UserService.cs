using Business.Factories;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Linq.Expressions;

namespace Business.Services;

public class UserService(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ResponseResult> AddUserInfoasync(UserRegistrationForm form)
    {
        try
        {
            var entity = UserFactory.Create(form);
            if (entity == null)
                return new ResponseResult { Succeeded = false, Message = "Invalid registration form." };

            var result = await _userRepository.AddAsync(entity);
            if (!result)
                return new ResponseResult { Succeeded = false, Message = "Something went wrong with creation.", StatusCode = 400 };

            return new ResponseResult { Succeeded = true, StatusCode = 200, Message = "Profile information was created successfully." };

        } catch (Exception ex)
        {
            return new ResponseResult { Succeeded = false, Message = ex.Message };
        }
    }

    public async Task<ResponseResult<User>> GetUserInfoAsync(Expression<Func<UserEntity, bool>> expression)
    {
        var entity = await _userRepository.GetAsync(expression);
        if (entity == null)
            return new ResponseResult<User> { Succeeded = false, Message = "User was not found.", StatusCode = 404 };

        var user = UserFactory.Create(entity);
        if (user == null)
            return new ResponseResult<User> { Succeeded = false };

        return new ResponseResult<User> { Succeeded = true, StatusCode = 200, Result = user };
    }

    public async Task<ResponseResult> UpdateProfileInfoAsync(User user)
    {
        var entity = UserFactory.Create(user);
        if (entity == null) 
            return new ResponseResult { Succeeded = false, Message = "Invalid user info" };

        var result  = await _userRepository.UpdateAsync(entity);
        if (!result)
            return new ResponseResult<User> { Succeeded = false, Message = "Couldn't update user information.", StatusCode = 400, };

        return new ResponseResult<User> { Succeeded = true, Message = "User information updated successfully.", StatusCode = 200 };
    }
}
