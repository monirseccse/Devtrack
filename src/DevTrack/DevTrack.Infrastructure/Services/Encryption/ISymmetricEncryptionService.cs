namespace DevTrack.Infrastructure.Services.Encryption
{
    public interface ISymmetricEncryptionService
    {
        Task<string> Encrypt(string data);
        Task<string> Decrypt(string encryptedData);
    }
}
