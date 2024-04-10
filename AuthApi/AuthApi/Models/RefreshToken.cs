using System;

namespace AuthApi.AuthApi.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiryTime {get; set; }

        public RefreshToken(string token)
        {
            Token = token;
            ExpiryTime = DateTime.UtcNow.AddDays(7);
        }

    }
}
