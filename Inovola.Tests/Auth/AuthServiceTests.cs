using FluentAssertions;
using Inovola.Application.DTOs.Auth;
using Inovola.Application.Interfaces;
using Inovola.Application.Services;
using Inovola.Domain.Entities;
using Inovola.Domain.Interfaces;
using Moq;

namespace Inovola.Application.Tests.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();

    private AuthService CreateService()
        => new AuthService(_userRepoMock.Object, _tokenServiceMock.Object);

    [Fact]
    public async Task RegisterAsync_NewUser_ReturnsToken()
    {
        // Arrange
        var service = CreateService();

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _tokenServiceMock
            .Setup(t => t.GenerateToken(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("fake-token");

        var request = new RegisterRequest
        {
            Email = "test@test.com",
            Password = "123456"
        };

        // Act
        var result = await service.RegisterAsync(request);

        // Assert
        result.Token.Should().Be("fake-token");
    }

    [Fact]
    public async Task RegisterAsync_ExistingUser_ThrowsException()
    {
        // Arrange
        var service = CreateService();

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User("test@test.com", "hash"));

        var request = new RegisterRequest
        {
            Email = "test@test.com",
            Password = "123456"
        };

        // Act
        Func<Task> act = () => service.RegisterAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange
        var password = "123456";
        var hash = Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(password)));

        var user = new User("test@test.com", hash);

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _tokenServiceMock
            .Setup(t => t.GenerateToken(It.IsAny<int>(), It.IsAny<string>()))
            .Returns("fake-token");

        var service = CreateService();

        var request = new LoginRequest
        {
            Email = "test@test.com",
            Password = password
        };

        // Act
        var result = await service.LoginAsync(request);

        // Assert
        result.Token.Should().Be("fake-token");
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsException()
    {
        // Arrange
        var user = new User("test@test.com", "wrong-hash");

        _userRepoMock
            .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        var service = CreateService();

        var request = new LoginRequest
        {
            Email = "test@test.com",
            Password = "123456"
        };

        // Act
        Func<Task> act = () => service.LoginAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
