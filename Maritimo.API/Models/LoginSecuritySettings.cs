namespace Maritimo.API.Models
{
    public class LoginSecuritySettings
    {

        public int MaxFailedAttempts { get; set; }
        public int LockMinutes { get; set; }

    }
}
