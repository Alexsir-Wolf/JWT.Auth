namespace Admin.Api.Infrastructure.Interfaces;

public interface IPasswordService
{
    string Encrypt(string password);
    string Decrypt(string encryptedPassword);
    
}