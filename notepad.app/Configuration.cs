using Microsoft.Extensions.Configuration;

namespace notepad.app;

public static class Configuration
{
    static public string ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new();
            try
            {

                var extension = "../notepad.webapi";
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), extension));
                configurationManager.AddJsonFile("appsettings.json");
            }
            catch
            {
                configurationManager.AddJsonFile("appsettings.json");
            }

            return configurationManager.GetConnectionString("SQLServer");
        }
    }
}