﻿using System.Security.Cryptography;

namespace Extensions;

public static class PasswordServices
{
    public static string GenerateSalt(int size)
    {
        var saltBytes = new byte[size];
        var provider = new RNGCryptoServiceProvider();
        provider.GetNonZeroBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);

    }
    public static string GenerateSaltedHash(string salt, string password)
    {
        return Convert.ToBase64String(GenerateHash(salt,password).GetBytes(256));
    }
    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        return Convert.ToBase64String(GenerateHash(storedSalt, enteredPassword).GetBytes(256)) == storedHash;
    }

    private static Rfc2898DeriveBytes GenerateHash(string salt, string password)
    {
        return new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000);
    }
}