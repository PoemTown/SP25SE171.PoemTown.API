﻿using System.Security.Cryptography;
using System.Text;

namespace PoemTown.Repository.Utils;

public static class PasswordHasher
{
    // Generate a random salt
    public static string GenerateSalt(int size = 32)
    {
        using var rng = new RNGCryptoServiceProvider();
        var saltBytes = new byte[size];
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    // Hash the password with the salt using SHA-256
    /// <summary>
    /// Hash the password with the salt using SHA-256
    /// </summary>
    /// <param name="password">User input password</param>
    /// <param name="salt">Account salt</param>
    /// <returns>SHA-256 Hash string</returns>
    public static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        // Combine the password and salt
        var saltedPassword = string.Concat(password, salt);

        // Convert the combined password and salt to a byte array
        var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);

        // Compute the hash
        var hashBytes = sha256.ComputeHash(saltedPasswordBytes);

        // Convert the hash to a Base64 string
        return Convert.ToBase64String(hashBytes);
    }

    // Verify if the provided password matches the hashed password
    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var hashOfEnteredPassword = HashPassword(enteredPassword, storedSalt);
        return hashOfEnteredPassword.Equals(storedHash);
    }
    
    public static string GenerateSecurePassword()
    {
        int length = 12;
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?{}[]|";
        StringBuilder password = new StringBuilder();
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] data = new byte[4];
            while (password.Length < length)
            {
                rng.GetBytes(data);
                var randomValue = BitConverter.ToUInt32(data, 0);
                password.Append(validChars[(int)(randomValue % validChars.Length)]);
            }
        }
        return password.ToString();
    }
}