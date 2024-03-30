namespace Auth.Auth.Interfaces
{
    public interface IEncryptionHandler
    {
        public string Encrypt(byte[] dataToEncrypt);
    }
}
