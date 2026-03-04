using System.Text.Json;

namespace AdoDb
{
    public static class Configuration
    {
        public static string GetConnectionString()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(appPath, "appsettings.json");

            if (!File.Exists(configPath))
            {
                string projectPath = Path.GetFullPath(Path.Combine(appPath, "..", "..", ".."));
                configPath = Path.Combine(projectPath, "appsettings.json");
            }

            string json = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

            if (config == null || !config.ContainsKey("ConnectionStrings"))
            {
                throw new InvalidOperationException("Invalid configuration file format");
            }

            if (!config["ConnectionStrings"].ContainsKey("DefaultConnection"))
            {
                throw new InvalidOperationException("DefaultConnection not found in configuration");
            }

            return config["ConnectionStrings"]["DefaultConnection"];
        }
    }
}