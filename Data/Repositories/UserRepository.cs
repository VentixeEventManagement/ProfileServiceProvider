using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class UserRepository(DataContext context, IAzureFileHandler fileHandler) : IUserRepository
{
    private readonly DataContext _context = context;
    private readonly IAzureFileHandler _fileHandler = fileHandler;
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

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        try
        {
            var entites = await _context.ProfileInfo.ToListAsync();
            return entites;

        } catch (Exception ex)
        {
            return null!;
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

            string? imageFileUri = null;
            if (user.ProfileImageUri != null)
            {
                imageFileUri = await _fileHandler.UploadFileAsync(user.ProfileImageUri);
            }
            
            if (imageFileUri != null)
            {
                existingEntity.ProfileImageUrl = imageFileUri;
            }
            
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

    public async Task<bool> DeleteAsync(string userId)
    {
        try
        {
            var existingEntity = await _context.ProfileInfo.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existingEntity == null)
                return false;

            _context.Remove(existingEntity);
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

        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
