using Data.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(UserEntity user);
        Task<bool> DeleteAsync(string userId);
        Task<bool> ExistsAsync(string userId);
        Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> expression);
        Task<bool> UpdateAsync(string userId, UserUpdateForm user);
    }
}