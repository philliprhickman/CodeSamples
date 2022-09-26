using ConsoleLibrary;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace ConfigurationFileManipulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleMethods.SetTitle("Configuration File Manipulation");

            ShowMenu();

            ConsoleMethods.WaitForKeypress();
        }

        #region App Settings Manipulation

        static void ListAppSettings()
        {
            var config = GetConfiguration();
            var appSettingsSection = config.GetSection("appSettings").CurrentConfiguration.AppSettings;
            var appSettings = appSettingsSection.Settings;

            foreach (var key in appSettings.AllKeys)
            {
                Console.WriteLine($"Key: {appSettings[key]} Value: {appSettings[key].Value}");
            }
        }

        static bool DoesAppSettingExist(string key)
        {
            var config = GetConfiguration();
            var appSettingsSection = config.GetSection("appSettings").CurrentConfiguration.AppSettings;
            var appSettings = appSettingsSection.Settings;

            return (appSettings[key]?.Value == null);
        }

        static void AddNewAppSetting(string key, string value)
        {
            var config = GetConfiguration();
            var appSettingsSection = config.GetSection("appSettings").CurrentConfiguration.AppSettings;
            var appSettings = appSettingsSection.Settings;

            if (appSettings[key]?.Value == null)
            {
                appSettings.Add(key, value);
                config.Save();
            }
        }

        static void RemoveExistingAppSetting(string key)
        {
            var config = GetConfiguration();
            var appSettingsSection = config.GetSection("appSettings").CurrentConfiguration.AppSettings;
            var appSettings = appSettingsSection.Settings;

            if (appSettings[key]?.Value != null)
            {
                appSettings.Remove(key);
                config.Save();
            }
        }

        static void UpdateExistingAppSetting(string key, string value)
        {
            var config = GetConfiguration();
            var appSettingsSection = config.GetSection("appSettings").CurrentConfiguration.AppSettings;
            var appSettings = appSettingsSection.Settings;

            if (appSettings[key]?.Value != null)
            {
                appSettings[key].Value = value;
                config.Save();
            }
        }

        #endregion

        #region Configuration

        static Configuration GetConfiguration()
        {
            var filePath = @"Files\web.config";
            var map = new ExeConfigurationFileMap { ExeConfigFilename = filePath };

            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        #endregion

        #region Connection Strings Manipulation

        static void ListConnectionString()
        {
            var config = GetConfiguration();
            var connectionStringsSection = config.GetSection("connectionStrings").CurrentConfiguration.ConnectionStrings;
            var connectionStrings = connectionStringsSection.ConnectionStrings;

            foreach (var connectionString in connectionStrings)
            {
                Console.WriteLine(connectionString.ToString());
            }
        }

        static void AddUserIDAndPasswordToConnectionString()
        {
            var config = GetConfiguration();

            config.ConnectionStrings.ConnectionStrings["Arena"].ConnectionString =
                RebuildConnectionString(config.ConnectionStrings.ConnectionStrings["Arena"].ConnectionString);

            config.Save();
        }

        static string RebuildConnectionString(string connectionString)
        {
            var csb = new SqlConnectionStringBuilder(connectionString);

            if (csb.UserID == "" && !csb.IntegratedSecurity)
            {
                csb.UserID = "Arena";
                csb.Password = "12345678";

                return csb.ConnectionString;
            }

            return connectionString;
        }

        static void EncryptConnectionStrings()
        {
            var config = GetConfiguration();
            var connectionStringsSection = config.GetSection("connectionStrings");

            if (connectionStringsSection != null && !connectionStringsSection.SectionInformation.IsProtected)
            {
                connectionStringsSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();
            }
        }

        static void DecryptConnectionStrings()
        {
            var config = GetConfiguration();
            var connectionStringsSection = config.GetSection("connectionStrings");

            if (connectionStringsSection != null && connectionStringsSection.SectionInformation.IsProtected)
            {
                connectionStringsSection.SectionInformation.UnprotectSection();
                config.Save();
            }
        }

        #endregion

        #region Menu Methods

        static void ShowMenu()
        {
            ConsoleMethods.AddBlankLine();
            ConsoleMethods.CenterText("1) List App Settings...........................");
            ConsoleMethods.CenterText("2) Does App Setting Exist?.....................");
            ConsoleMethods.CenterText("3) Add App Setting.............................");
            ConsoleMethods.CenterText("4) Remove App Setting..........................");
            ConsoleMethods.CenterText("5) Update App Setting..........................");
            ConsoleMethods.CenterText("6) List Connection Strings.....................");
            ConsoleMethods.CenterText("7) Add UserID and Password to Connection String");
            ConsoleMethods.CenterText("8) Encrypt Connection String...................");
            ConsoleMethods.CenterText("9) Decrypt Connection String...................");
            ConsoleMethods.CenterText("0) Exit........................................");
        }

        #endregion
    }
}
