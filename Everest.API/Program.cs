namespace Everest.API
{
    #region << Using >>

    using Incoding.Core.Block.Logging;
    using Incoding.Core.Block.Logging.Core;
    using Microsoft.AspNetCore.Hosting;

    #endregion

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                LoggingFactory.Instance.LogException(LogType.Debug, e);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                                              {
                                                  webBuilder.ConfigureAppConfiguration(builder =>
                                                                                               builder.AddJsonFile("connectionStrings.json", optional: true)
                                                                                                      .AddJsonFile("appSettings.json", optional: true)
                                                                                                      .AddJsonFile("customSettings.json", optional: true));
                                                  webBuilder.UseStartup<Startup>();
                                              });
    }
}