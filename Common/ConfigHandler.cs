using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace RailWorks.Common
{
    class ConfigHandler
    {
        private IConfiguration _config = null;
        public IConfiguration Configuration
        {
            get { return _config; }
        }

        public ConfigHandler()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets("a59f52bb-3247-491a-8050-1d79aee5b6a6");
            _config = builder.Build();
        }
    }
}