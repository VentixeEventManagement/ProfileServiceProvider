using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> AddAsync(UserEntity user)
    {
        try
        {
            if (user == null)
            {
                return false;
            }

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<UserEntity?> GetAsync(Expression<Func<UserEntity, bool>> expression)
    {
        try
        {
            if (expression == null)
            {
                return null;
            }

            var entity = await _context.Users.FirstOrDefaultAsync(expression);
            if (entity == null)
            {
                return null;
            }

            return entity;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<int> UpdateAsync(UserEntity user)
    {
        try
        {
            _context.Update(user);
            var result = await _context.SaveChangesAsync();
            return result;

        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<int> DeleteAsync(UserEntity user)
    {
        try
        {
            _context.Remove(user);
            var result = await _context.SaveChangesAsync();
            return result;

        }
        catch (Exception ex)
        {
            return default;
        }
    }
}
