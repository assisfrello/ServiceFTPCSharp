using Ftp.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Ftp.CrossCutting.IoC
{
    public class DependencyInjector
    {
        public static void RegisterServices(IServiceCollection services)
        {
            AddOthers(services);
        }

        private static void AddOthers(IServiceCollection services)
        {
            services.AddScoped<ProcessadorFtp>();
        }
    }
}