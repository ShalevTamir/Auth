using AuthApi.AuthApi.Interfaces;
using AuthApi.AuthApi.Models;
using System;
using System.Collections.Generic;

namespace AuthApi.AuthApi.Services
{
    public class RefreshTokenHandler: IRefreshTokenHandler
    {
        // key - username
        // value - refresh token of the user
        private Dictionary<string, RefreshToken> _refreshTokens;

        public RefreshTokenHandler()
        {
            _refreshTokens = new Dictionary<string, RefreshToken>();
        }

        public void SaveRefreshToken(string username, string refreshToken)
        {
            _refreshTokens[username] = new RefreshToken(refreshToken);
        }

        public bool IsValid(string username, string refreshToken)
        {
            return _refreshTokens.ContainsKey(username) && 
                _refreshTokens[username].Token == refreshToken &&
                IsValidExpirationDate(_refreshTokens[username].ExpiryTime);
        }

        public bool DeleteRefreshToken(string username)
        {
            if (_refreshTokens.ContainsKey(username))
            {
                _refreshTokens.Remove(username);
                return true;
            }
            return false;
        }

        private bool IsValidExpirationDate(DateTime expirationDate)
        {
            return DateTime.Now <= expirationDate;
        }
    }
}
