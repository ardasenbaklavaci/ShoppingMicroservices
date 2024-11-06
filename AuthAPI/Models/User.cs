using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AuthAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        public byte[] PasswordHash { get; set; } // Store hashed passwords only

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } // e.g., "User" or "Admin"

        public static byte[] HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                return sha256.ComputeHash(bytes);
            }
        }

        public bool VerifyPassword(string password)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword.SequenceEqual(PasswordHash);
        }
    }
}
