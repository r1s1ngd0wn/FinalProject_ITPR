using System.Configuration;

namespace DACK_ITPROJECT.Data
{
    public static class DbConfig
    {
        // Centralized provider for the connection string.
        // Keeps legacy hardcoded value as a fallback so code keeps working
        // if App.config entry is missing.
        public static readonly string ConnectionString;

        static DbConfig()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["PhoneStore"]?.ConnectionString
                ?? @"Data Source=.;Initial Catalog=PhoneStore_V6;Integrated Security=True";
        }
    }
}