using System.ComponentModel.DataAnnotations;

namespace Inovola.Domain.Entities;

public class User
{
    public int Id { get; private set; }

    [Required]
    [MaxLength(256)]
    public string Email { get; private set; } = null!;

    [Required]
    public string PasswordHash { get; private set; } = null!;

    private User() { }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
}
