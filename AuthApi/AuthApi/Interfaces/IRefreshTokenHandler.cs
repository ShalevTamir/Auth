namespace AuthApi.AuthApi.Interfaces
{
    public interface IRefreshTokenHandler
    {
        void SaveRefreshToken(string username, string refreshToken);
        bool IsValid(string username, string refreshToken);
        bool DeleteRefreshToken(string username);
    }
}
