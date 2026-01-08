namespace Inovola.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(int userId, string email);
}
