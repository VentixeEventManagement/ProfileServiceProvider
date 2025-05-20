using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddAsync(UserEntity user);
        Task<int> DeleteAsync(UserEntity user);
        Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> expression);
        Task<int> UpdateAsync(UserEntity user);
    }
}