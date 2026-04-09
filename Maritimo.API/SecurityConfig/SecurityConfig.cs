using Maritimo.API.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Maritimo.API.SecurityConfig
{
    public static class SecurityConfig
    {
        public static LoginSecuritySettings GetSettings()
        {

            var json = File.ReadAllText("securitySettings.json");


            dynamic config = JsonConvert.DeserializeObject(json);

            return new LoginSecuritySettings
            {
                MaxFailedAttempts = config.LoginSecurity.MaxFailedAttempts,
                LockMinutes = config.LoginSecurity.LockMinutes
            };
        }
    }
}
