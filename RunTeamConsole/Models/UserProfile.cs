using System;
using System.Collections.Generic;
using System.Text;

namespace RunTeamConsole.Models
{
    public static class UserProfile
    {
        public static string UserName { get; set; }
        public static string Email { get; set; }

        public static string ItUser { get; set; }
        public static string Department { get; set; }
        public static string Host { get; set; }
        public static string Domain { get; set; }
        public static Credentials CachedCredentials { get; set; }
    }
}
