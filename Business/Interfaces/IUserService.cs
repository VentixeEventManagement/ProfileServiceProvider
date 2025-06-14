﻿using Business.Models;
using Data.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task<ResponseResult> AddUserInfoasync(UserRegistrationForm form);
        Task<ResponseResult> DeleteProfileInfoAsync(string userId);
        Task<ResponseResult<IEnumerable<User>>> GetAllProfilesAsync();
        Task<ResponseResult<User>> GetUserInfoAsync(Expression<Func<UserEntity, bool>> expression);
        Task<ResponseResult> UpdateProfileInfoAsync(string userId, UserUpdateForm user);
    }
}