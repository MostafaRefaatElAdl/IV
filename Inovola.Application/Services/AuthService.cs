using Inovola.Application.DTOs.Auth;
using Inovola.Application.Interfaces;
using Inovola.Domain.Entities;
using Inovola.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Inovola.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("User already exists");

        var passwordHash = HashPassword(request.Password);

        var user = new User(request.Email, passwordHash);
        await _userRepository.AddAsync(user);

        var token = _tokenService.GenerateToken(user.Id, user.Email);

        return new AuthResponse
        {
            Token = token
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new InvalidOperationException("Invalid credentials");

        var passwordHash = HashPassword(request.Password);
        if (user.PasswordHash != passwordHash)
            throw new InvalidOperationException("Invalid credentials");

        var token = _tokenService.GenerateToken(user.Id, user.Email);

        return new AuthResponse
        {
            Token = token
        };
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}
