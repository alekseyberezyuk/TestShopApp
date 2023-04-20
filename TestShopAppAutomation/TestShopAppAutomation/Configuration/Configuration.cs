
using Newtonsoft.Json;

namespace TestShopAppAutomation
{
    public class Configuration
    {
        private const string CONF_FILE_PATH = "Configuration/configuration.json";

        public static Configuration Instance { get; private set; }

        public string baseUrl { get; set; }
        public string loginPageUrl { get; set; }
        public string mainPageUrl { get; set; }
        public TestUserAccountCreds testUserAccountCreds { get; set; }
        public ApiSettings apiSettings { get; set; }

        private Configuration()
        {
        }

        internal static void Load()
        {
            if (Instance == null)
            {
                Instance = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(CONF_FILE_PATH));
            }
        }
    }

    public class TestUserAccountCreds
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class ApiSettings
    {
        public string baseUrl { get; set; }
        public string auth { get; set; }
    }
}
