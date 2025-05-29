using Business.Factories;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;

namespace ProfileServiceProvider.Test.Business_Tests;

public class UserService_Tests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAzureFileHandler> _fileHandlerMock;
    private readonly UserService _userService;

    public UserService_Tests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _fileHandlerMock = new Mock<IAzureFileHandler>();
        _userService = new UserService(_userRepositoryMock.Object, _fileHandlerMock.Object);
    }

    // Add Profile information ----------------------------------------------

    [Fact]
    public async Task AddUserInfoAsync_ShouldReturnSuccessAnd201_WhenValidFormWithImage()
    {
        // Arrange
        var mockIformFile = new Mock<IFormFile>();

        var form = new UserRegistrationForm 
        {
            UserId = "123456",
            ProfileImageUri = mockIformFile.Object,
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "1234567890",
        };

        var uploadePath = "uploaded/image/path.jpg";
        var userEntity = new UserEntity();

        _fileHandlerMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(uploadePath);
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

        // Act
        var result = await _userService.AddUserInfoasync(form);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(201, result.StatusCode);
        Assert.Equal("Profile information was created successfully.", result.Message);

        _fileHandlerMock.Verify(x => x.UploadFileAsync(form.ProfileImageUri), Times.Once());
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserEntity>()), Times.Once());
    }

    [Fact]
    public async Task AddUserInfoAsync_ShouldReturnFalseAnd500_WhenUploadImageThrowsExeption()
    {
        // Arrange
        var mockIformFile = new Mock<IFormFile>();

        var form = new UserRegistrationForm
        {
            UserId = "123456",
            ProfileImageUri = mockIformFile.Object,
        };

        _fileHandlerMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>())).ThrowsAsync(new Exception("Upload failed"));

        // Act
        var result = await _userService.AddUserInfoasync(form);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Upload failed", result.Message);
    }

    [Fact]
    public async Task AddUserInfoAsync_ShouldReturnFalseAnd500_IfUserIdIsNullOrEmpty()
    {
        // Arrange
        var mockIformFile = new Mock<IFormFile>();

        var form = new UserRegistrationForm
        {
            UserId = "",
            ProfileImageUri = mockIformFile.Object,
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "1234567890",
        };

        var uploadePath = "uploaded/image/path.jpg";
        var userEntity = new UserEntity();

        _fileHandlerMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(uploadePath);
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<UserEntity>())).ReturnsAsync(true);

        // Act
        var result = await _userService.AddUserInfoasync(form);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("User id cannot be null or empty.", result.Message);
    }

    // Get all profiles ----------------------------------------------

    [Fact] 
    public async Task GetAllProfiles_ShouldReturnTrue_WhenProfilesExists()
    {
        // Arrange
        var userEntities = new List<UserEntity>
            {
                new UserEntity { Id = "userId-1", UserId = "user1", FirstName = "Björn", LastName = "Åhström", PhoneNumber = "1234567890", ProfileImageUrl = "url1" },
                new UserEntity { Id = "userId-2", UserId = "user2", FirstName = "Desirée", LastName = "Åhström", PhoneNumber = "0987654321", ProfileImageUrl = "url2" }
            };

        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(userEntities);

        // Act
        var result = await _userService.GetAllProfilesAsync();

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Result);
        Assert.Equal(2, result.Result.Count());
        Assert.Contains(result.Result, x => x.Id == "userId-1");
        Assert.Contains(result.Result, x => x.Id == "userId-2");
    }

    [Fact]
    public async Task GetAllProfiles_ShouldReturnFalse_WhenEntitesAreNull()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync((List<UserEntity>)null!);

        // Act
        var result = await _userService.GetAllProfilesAsync();

        // Assert
        Assert.False(result.Succeeded);
        Assert.Null(result.Result);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Something went wrong when fetching profile information.", result.Message);

    }


    // Get profile info from one user ----------------------------------------------

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnTrue_WhenProfileExists()
    {
        // Arrange
        string userId = "user1";

        var userEntities = new UserEntity
            {
                Id = "userId-1",
                UserId = "user1", 
                FirstName = "Björn", 
                LastName = "Åhström",
                PhoneNumber = "1234567890", 
                ProfileImageUrl = "url1"
            };

        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserEntity, bool>>>())).ReturnsAsync(userEntities);

        // Act
        var result = await _userService.GetUserInfoAsync(x => x.UserId == userId);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(200, result.StatusCode);
        Assert.NotNull(result.Result);
        Assert.Equal(userId, result.Result.UserId);
        Assert.Equal("Björn", result.Result.FirstName);
    }

    [Fact]
    public async Task GetUserInfoAsync_ShouldReturnFalse_WhenProfileNotFound()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserEntity, bool>>>())).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _userService.GetUserInfoAsync(x => x.UserId == "No user id");

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(404, result.StatusCode);
        Assert.Null(result.Result);
        Assert.Equal("User was not found.", result.Message);
    }

    // Update profile information ---------------------------------------------- 

    [Fact]
    public async Task UpdateProfileInfoAsync_ShouldReturnTrue_WhenProfileWasUpdatedSuccessfully()
    {
        // Arrange
        var mockIformFile = new Mock<IFormFile>();
        string userId = "user1";

        var form = new UserUpdateForm
        {
            UserId = "user1",
            ProfileImageUri = mockIformFile.Object,
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "1234567890",
        };

        var uploadePath = "uploaded/image/path.jpg";

        _fileHandlerMock.Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(uploadePath);
        _userRepositoryMock.Setup(x => x.UpdateAsync(userId, form)).ReturnsAsync(true);

        // Act
        var result = await _userService.UpdateProfileInfoAsync(userId, form);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("User information updated successfully.", result.Message);

        _userRepositoryMock.Verify(x => x.UpdateAsync(userId, form), Times.Once());
    }

    [Fact]
    public async Task UpdateProfileInfoAsync_ShouldReturnFalse_WhenUseIdIsInvalid()
    {
        // Arrange
        var mockIformFile = new Mock<IFormFile>();
        string userId = "invalid-user-id";

        var form = new UserUpdateForm
        {
            UserId = "user1",
            ProfileImageUri = mockIformFile.Object,
            FirstName = "Björn",
            LastName = "Åhström",
            PhoneNumber = "1234567890",
        };

        _userRepositoryMock.Setup(x => x.UpdateAsync(userId, form)).ReturnsAsync(false);

        // Act
        var result = await _userService.UpdateProfileInfoAsync(userId, form);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("Invalid user id.", result.Message);

        _userRepositoryMock.Verify(x => x.UpdateAsync(userId, form), Times.Once());
    }

    [Fact]
    public async Task UpdateProfileInfoAsync_ShouldReturnFalse_WhenUserFormIsNull()
    {
        // Arrange
        string userId = "user1";

        UserUpdateForm form = null!;

        // Act
        var result = await _userService.UpdateProfileInfoAsync(userId, form);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("User is null", result.Message);

        _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<UserUpdateForm>()), Times.Never());
    }

    // Delete profile information ---------------------------------------------- 

    [Fact]
    public async Task DeleteProfileInfoAsync_ShouldReturnTrue_WhenProfileInfoIsDeleted()
    {
        // Arrange
        string userId = "user1";

        _userRepositoryMock.Setup(x => x.DeleteAsync(userId)).ReturnsAsync(true);

        // Act
        var result = await _userService.DeleteProfileInfoAsync(userId);

        // Assert 
        Assert.True(result.Succeeded);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal("User information deleted successfully.", result.Message);

        _userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once());
    }

    [Fact]
    public async Task DeleteProfileInfoAsync_ShouldReturnFalse_WhenUserIsNull()
    {
        // Arrange
        string userId = null!;

        _userRepositoryMock.Setup(x => x.DeleteAsync(userId)).ReturnsAsync(false);

        // Act
        var result = await _userService.DeleteProfileInfoAsync(userId);

        // Assert 
        Assert.False(result.Succeeded);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal("User id is null", result.Message);

        _userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Never());
    }

    [Fact]
    public async Task DeleteProfileInfoAsync_ShouldReturnFalse_WhenUserNotFound()
    {
        // Arrange
        string userId = "";

        _userRepositoryMock.Setup(x => x.DeleteAsync(userId)).ReturnsAsync(false);

        // Act
        var result = await _userService.DeleteProfileInfoAsync(userId);

        // Assert 
        Assert.False(result.Succeeded);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("Couldn't delete user information.", result.Message);

        _userRepositoryMock.Verify(x => x.DeleteAsync(userId), Times.Once());
    }
}