using System;

namespace JoggingApp.Users
{
    public class UserAuthResponse
    {
        public UserAuthResponse(string email, string token)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
