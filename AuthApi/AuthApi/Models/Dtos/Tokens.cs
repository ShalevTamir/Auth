using System.ComponentModel.DataAnnotations;

namespace AuthApi.AuthApi.Models.Dtos
{
    public class Tokens
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
