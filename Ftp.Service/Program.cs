using System;
using Ftp.CrossCutting.IoC;
using Ftp.Domain;
using Microsoft.Extensions.DependencyInjection;
using Topshelf;

namespace Ftp.Service
{
    class Program
    {
        private static void Main()
        {
            var serviceProvider = new ServiceCollection();
            DependencyInjector.RegisterServices(serviceProvider);
            
            new Service(new ProcessadorFtp()).Start();
            
            var hostFactory = HostFactory.Run(o =>
            {
                o.Service<Service>(s =>
                {
                    s.ConstructUsing(_ => 
                        new Service(
                            new ProcessadorFtp()));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                o.RunAsLocalService();

                o.SetDescription("Serviço de FTP");
                o.SetDisplayName("FTP Service");
                o.SetServiceName("FTP Service");
            });

            var exitCode = (int) Convert.ChangeType(hostFactory, hostFactory.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}