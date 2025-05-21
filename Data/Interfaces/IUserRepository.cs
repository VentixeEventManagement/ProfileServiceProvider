using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(UserEntity user);
        Task<bool> DeleteAsync(UserEntity user);
        Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> expression);
        Task<bool> UpdateAsync(UserEntity user);
    }
}