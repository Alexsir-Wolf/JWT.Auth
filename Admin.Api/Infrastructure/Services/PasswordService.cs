using System.Security.Cryptography;
using System.Text;
using Admin.Api.Infrastructure.Interfaces;

namespace Admin.Api.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly IConfiguration _configuration;

    public PasswordService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Encrypt(string password)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = GenerateRandomKey(int.Parse(_configuration["HashKey:keySize"] ?? "32"));
        aesAlg.GenerateIV();

        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write))
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(password);
        }

        var encryptedBytes = msEncrypt.ToArray();
        var resultWithIV = aesAlg.IV.Concat(encryptedBytes).ToArray();
    
        return Convert.ToBase64String(resultWithIV);
    }


    public string Decrypt(string encryptedPassword)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = GenerateRandomKey(int.Parse(_configuration["HashKey:keySize"] ?? "32"));
        aesAlg.GenerateIV();
        
        using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        using var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedPassword));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        return srDecrypt.ReadToEnd();
    }
    
    private static byte[] GenerateRandomKey(int bytesSize)
    {
        var key = new byte[bytesSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return key;
    }
}