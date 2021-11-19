using System;
using Dapper;

namespace TestShopApplication.Dal
{
    public static class DapperConfig
    {
        private static volatile bool _isConfigured;

        public static void ConfigureDapper()
        {
            if (!_isConfigured)
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
            }
        }
    }
}
