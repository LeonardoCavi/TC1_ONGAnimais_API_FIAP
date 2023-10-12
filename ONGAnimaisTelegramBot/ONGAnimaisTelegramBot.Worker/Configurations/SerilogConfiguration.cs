using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Worker.Configurations
{
    public static class SerilogConfiguration
    {
        public static void AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                   .ReadFrom
                   .Configuration(configuration)
                   .CreateLogger();
        }
    }
}
