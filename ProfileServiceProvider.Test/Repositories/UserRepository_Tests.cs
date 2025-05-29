using Data.Contexts;
using Data.Entities;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ProfileServiceProvider.Test.Repositories;

public class UserRepository_Tests
{
    private readonly DbContextOptions<DataContext> _options;
    private DataContext? _context;

    public UserRepository_Tests()
    {
        _options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
    }

    private UserRepository CreateRepositoryWithContext()
    {
        _context = new DataContext(_options);
        var mockFileHandler = new Mock<IAzureFileHandler>();
        var repository = new UserRepository(_context, mockFileHandler.Object);

        return new UserRepository(_context, mockFileHandler.Object);
    }

    // Add profile information

    [Fact]
    public async Task AddAsync_ShouldReturnTrue_WhenUserIsAdded()
    {
        // Arrange
        var repository = CreateRepositoryWithContext();

        var user = new UserEntity
        {
            Id = "user1",
            UserId = "UserId-1",
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "1234567890",
            ProfileImageUrl = "avatar.svg"
        };

        // Act
        var result = await repository.AddAsync(user);

        // Assert
        Assert.True(result);

        var addedUser = await _context.ProfileInfo.FirstOrDefaultAsync(x => x.UserId == "UserId-1");
        Assert.NotNull(addedUser);
        Assert.Equal("Björn", addedUser.FirstName);
        Assert.Equal("Åhström", addedUser.LastName);

        _context!.Dispose();
    }

    [Fact]
    public async Task AddAsync_ShouldReturnFalse_WhenUserIsnull()
    {
        // Arrange 
        var repository = CreateRepositoryWithContext();

        // Act
        var result = await repository.AddAsync(null!);

        // Assert
        Assert.False(result);

        _context!.Dispose();
    }

    // Get all profiles information

    [Fact]  
    public async Task GetAllAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange 
        var repository = CreateRepositoryWithContext();

        var users = new List<UserEntity>
        {
            new() { UserId = "user1", FirstName = "Björn", LastName = "Åhström" },
            new() { UserId = "user2", FirstName = "Desirée", LastName = "Åhström" }
        };

        await _context!.AddRangeAsync(users);
        await _context!.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, u => u.UserId == "user1" && u.FirstName == "Björn");
        Assert.Contains(resultList, u => u.UserId == "user2" && u.FirstName == "Desirée");

        _context!.Dispose();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoUserExists()
    {
        // Arrange 
        var repository = CreateRepositoryWithContext();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _context!.Dispose();
    }

    // Get profile information by id

    [Fact]
    public async Task GetAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var repository = CreateRepositoryWithContext();

        var user = new UserEntity
        {
            UserId = "user1",
            FirstName = "Björn",
            LastName = "Åhström"
        };

        await _context!.AddAsync(user);
        await _context!.SaveChangesAsync();

        // Act
        var result = await repository.GetAsync(u => u.UserId == "user1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Björn", result!.FirstName);
        Assert.Equal("Åhström", result.LastName);

        _context!.Dispose();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var repository = CreateRepositoryWithContext();

        // Act
        var result = await repository.GetAsync(u => u.UserId == "");

        // Assert
        Assert.Null(result);

        _context!.Dispose();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenExpressionIsNull()
    {
        // Arrange
        var repository =CreateRepositoryWithContext();

        // Act
        var result = await repository.GetAsync(null!);

        // Assert
        Assert.Null(result);

        _context!.Dispose();
    }

    // Update profile information

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenUserIsUpdated()
    {
        // Arrange
        var repository = CreateRepositoryWithContext();

        var userEntity = new UserEntity
        {
            UserId = "user1",
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "0701234567"
        };
        await _context!.AddAsync(userEntity);
        await _context!.SaveChangesAsync();

        var userUpdate = new UserUpdateForm
        {
            FirstName = "Desirée",
            LastName = "Åhström",
            PhoneNumber = "0736123456",
            ProfileImageUri = null
        };

        // Act
        var result = await repository.UpdateAsync("user1", userUpdate);

        // Assert
        Assert.True(result);
        var updatedUser = await _context!.ProfileInfo.FirstOrDefaultAsync(u => u.UserId == "user1");
        Assert.NotNull(updatedUser);
        Assert.Equal("Desirée", updatedUser!.FirstName);

        _context.Dispose();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_AndUpdateProfileImageUrl_WhenProfileImageUriProvided()
    {
        // Arrange
        var mockFileHandler = new Mock<IAzureFileHandler>();
        mockFileHandler.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>())).ReturnsAsync("new-avatar.jpg");

        _context = new DataContext(_options);

        var existingUser = new UserEntity
        {
            UserId = "user1",
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "0701234567",
            ProfileImageUrl = "old-avatar.jpg"
        };

        await _context.ProfileInfo.AddAsync(existingUser);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context, mockFileHandler.Object);

        var mockIFormFile = new Mock<IFormFile>();
        var userUpdate = new UserUpdateForm
        {
            FirstName = "Desirée",
            LastName = "Åhström",
            PhoneNumber = "0736123456",
            ProfileImageUri = mockIFormFile.Object
        };

        // Act
        var result = await repository.UpdateAsync("user1", userUpdate);

        // Assert
        Assert.True(result);

        var updatedUser = await _context.ProfileInfo.FirstOrDefaultAsync(u => u.UserId == "user1");
        Assert.NotNull(updatedUser);
        Assert.Equal("Desirée", updatedUser!.FirstName);
        Assert.Equal("Åhström", updatedUser.LastName);
        Assert.Equal("0736123456", updatedUser.PhoneNumber);
        Assert.Equal("new-avatar.jpg", updatedUser.ProfileImageUrl);

        _context.Dispose();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var repository = CreateRepositoryWithContext();

        var userUpdate = new UserUpdateForm
        {
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "0736123456",
            ProfileImageUri = null
        };

        // Act
        var result = await repository.UpdateAsync("nonexistent", userUpdate);

        // Assert
        Assert.False(result);

        _context!.Dispose();
    }

    // Delete profile information

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userId = "user1";
        var user = new UserEntity
        {
            UserId = userId,
            FirstName = "Björn"
        };

        _context = new DataContext(_options);
        await _context.ProfileInfo.AddAsync(user);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context, Mock.Of<IAzureFileHandler>());

        // Act
        var result = await repository.DeleteAsync(userId);

        // Assert
        Assert.True(result);

        var deletedUser = await _context.ProfileInfo.FirstOrDefaultAsync(u => u.UserId == userId);
        Assert.Null(deletedUser);

        _context.Dispose();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "";
        _context = new DataContext(_options);

        var repository = new UserRepository(_context, Mock.Of<IAzureFileHandler>());

        // Act
        var result = await repository.DeleteAsync(userId);

        // Assert
        Assert.False(result);

        _context.Dispose();
    }


    // Profile exists
    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userId = "user";
        var user = new UserEntity { UserId = userId };

        _context = new DataContext(_options);
        await _context.ProfileInfo.AddAsync(user);
        await _context.SaveChangesAsync();

        var repository = new UserRepository(_context, Mock.Of<IAzureFileHandler>());

        // Act
        var result = await repository.ExistsAsync(userId);

        // Assert
        Assert.True(result);

        _context.Dispose();
    }
}
