using System.Security.Cryptography;

namespace BookingApp.Infrastructure;

public class PasswordHasher
{
    public static string Hash(string password)
    {
        //pbkdf2 builtin, for learning and production basic
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations: 100000,
            hashAlgorithm: HashAlgorithmName.SHA256,
            outputLength: 32
            );

        //store as: iterations.salt.hash (Base64)
        return $"100000.{Convert.ToBase64String(hash)}.{Convert.ToBase64String(salt)}";
    }

    public static bool Verify(string password, string stored)
    {
        var parts = stored.Split(".");
        if (parts.Length != 3) return false;

        if(!int.TryParse(parts[0], out var iterations)) return false;
        var salt = Convert.FromBase64String(parts[1]);
        var expectedHash = Convert.FromBase64String(parts[2]);
        
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            32
            );
        
        return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
    }
}