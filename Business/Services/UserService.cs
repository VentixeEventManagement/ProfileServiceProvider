﻿using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IAzureFileHandler fileHandler) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAzureFileHandler _fileHandler = fileHandler;

    public async Task<ResponseResult> AddUserInfoasync(UserRegistrationForm form)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(form.UserId))
                return new ResponseResult { Succeeded = false, Message = "User id cannot be null or empty.", StatusCode = 400 };

                string? imageFileUri = null;

            if (form.ProfileImageUri != null)
            {
                imageFileUri = await _fileHandler.UploadFileAsync(form.ProfileImageUri);
            }

            var entity = UserFactory.Create(form, imageFileUri);
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

    public async Task<ResponseResult<IEnumerable<User>>> GetAllProfilesAsync()
    {
        var users = new List<User>();

        try
        {
            var entities = await _userRepository.GetAllAsync();
            if (entities == null)
                return new ResponseResult<IEnumerable<User>> { Succeeded = false, StatusCode = 500, Message = "Something went wrong when fetching profile information." };

            foreach (var entity in entities)
            {
                users.Add(UserFactory.Create(entity));
            }

            return new ResponseResult<IEnumerable<User>> { Succeeded = true, StatusCode = 200, Result = users };
        } catch (Exception ex)
        {
            return new ResponseResult<IEnumerable<User>> { Succeeded = false, Message = ex.Message, StatusCode = 500};
        }
    }

    public async Task<ResponseResult<User>> GetUserInfoAsync(Expression<Func<UserEntity, bool>> expression)
    {
        try
        {
            var entity = await _userRepository.GetAsync(expression);
            if (entity == null)
                return new ResponseResult<User> { Succeeded = false, Message = "User was not found.", StatusCode = 404 };

            var user = UserFactory.Create(entity);
            if (user == null)
                return new ResponseResult<User> { Succeeded = false, Message = "Failed to map user.", StatusCode = 500 };

            return new ResponseResult<User> { Succeeded = true, StatusCode = 200, Result = user };
        } catch (Exception ex)
        {
            return new ResponseResult<User> { Succeeded = false, Message = ex.Message, StatusCode = 500 };
        }
    }

    public async Task<ResponseResult> UpdateProfileInfoAsync(string userId, UserUpdateForm user)
    {
        try
        {
            if (userId == null)
                return new ResponseResult { Succeeded = false, Message = "User id is null", StatusCode = 400 };

            if (user == null)
                return new ResponseResult { Succeeded = false, Message = "User is null", StatusCode = 400 };

            var updated = await _userRepository.UpdateAsync(userId, user);
            if (!updated)
                return new ResponseResult<User> { Succeeded = false, Message = "Invalid user id.", StatusCode = 400, };

            return new ResponseResult<User> { Succeeded = true, Message = "User information updated successfully.", StatusCode = 200 };
        }
        catch (Exception ex)
        {
            return new ResponseResult { Succeeded = false, Message = ex.Message, StatusCode = 500 };
        }
    }

    public async Task<ResponseResult> DeleteProfileInfoAsync(string userId)
    {
        try
        {
            if (userId == null)
                return new ResponseResult { Succeeded = false, Message = "User id is null", StatusCode = 400 };

            var deleted = await _userRepository.DeleteAsync(userId);
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
