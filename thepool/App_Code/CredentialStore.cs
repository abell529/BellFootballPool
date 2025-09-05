using System;
using System.Configuration;
using System.Net;

public static class CredentialStore
{
        public static string ApiUsername =>
            Environment.GetEnvironmentVariable("POOL_API_USERNAME") ??
            ConfigurationManager.AppSettings["ApiUsername"];

        public static string ApiPassword =>
            Environment.GetEnvironmentVariable("POOL_API_PASSWORD") ??
            ConfigurationManager.AppSettings["ApiPassword"];

        public static string DbUser =>
            Environment.GetEnvironmentVariable("POOL_DB_USER") ??
            ConfigurationManager.AppSettings["DbUser"];

        public static string DbPassword =>
            Environment.GetEnvironmentVariable("POOL_DB_PASSWORD") ??
            ConfigurationManager.AppSettings["DbPassword"];

        public static string EmailAddress =>
            Environment.GetEnvironmentVariable("POOL_EMAIL_ADDRESS") ??
            ConfigurationManager.AppSettings["EmailAddress"];

        public static string EmailPassword =>
            Environment.GetEnvironmentVariable("POOL_EMAIL_PASSWORD") ??
            ConfigurationManager.AppSettings["EmailPassword"];

        public static string CcAddress =>
            Environment.GetEnvironmentVariable("POOL_CC_ADDRESS") ??
            ConfigurationManager.AppSettings["CcAddress"];

        public static NetworkCredential ApiCredential =>
            new NetworkCredential(ApiUsername, ApiPassword);

        public static NetworkCredential EmailCredential =>
            new NetworkCredential(EmailAddress, EmailPassword);

        public static string ScoresConnectionString =>
            $"Server=mysql24.ezhostingserver.com;Database=bfscoresDB;User ID={DbUser};Password={DbPassword};";

        public static string PoolConnectionString =>
            $"Server=mysql24.ezhostingserver.com;Database=bfpoolDB;User ID={DbUser};Password={DbPassword};";
}
