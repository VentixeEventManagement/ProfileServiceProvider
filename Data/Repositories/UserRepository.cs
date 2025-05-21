using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
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

            var entity = await _context.ProfileInfo.FirstOrDefaultAsync(expression);
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

    public async Task<bool> UpdateAsync(string userId, UserUpdateForm user)
    {
        try
        {
            var existingEntity = await _context.ProfileInfo.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingEntity == null) 
                return false;

            existingEntity.ProfileImageUrl = user.ProfileImageUrl;
            existingEntity.FirstName = user.FirstName;
            existingEntity.LastName = user.LastName;

            _context.Update(existingEntity);
            var result = await _context.SaveChangesAsync();
            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(UserEntity user)
    {
        try
        {
            _context.Remove(user);
            var result = await _context.SaveChangesAsync();
            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> ExistsAsync(string userId)
    {
        try
        {
            var entity = await GetAsync(x => x.UserId == userId);
            if (entity == null)
                return false;

            return true;

        } catch (Exception ex)
        {
            return false;
        }
    }
}
