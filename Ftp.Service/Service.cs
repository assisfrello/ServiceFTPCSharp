using System;
using System.Configuration;
using System.IO;
using System.Timers;
using Ftp.CrossCutting.Constantes;
using Ftp.Domain;
using Ftp.Domain.Model;
using Newtonsoft.Json;
using Serilog;

namespace Ftp.Service
{
    public class Service
    {
        private static bool Running { get; set; }
        
        private readonly Timer _timer;
        private readonly ProcessadorFtp _processadorFtp;

        public Service(ProcessadorFtp processadorFtp)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("log/log.txt").CreateLogger();
            
            _processadorFtp = processadorFtp;
            
            var interval = ConfigurationManager.AppSettings[StringConstantes.Intervalo];

            _timer = new Timer(interval != null ? Convert.ToInt32(interval) : 86400000) {AutoReset = true};
            _timer.Elapsed += Executar;
        }

        public void Start()
        {
            _timer.Start();
        }
        
        public void Stop()
        {
            _timer.Stop();
        }

        private async void Executar(object source, ElapsedEventArgs e)
        {
            try
            {
                if (Running)
                    return;

                Running = true;
                
                var servidores = BuscaServidores();
                
                Log.Information("Iniciando o processamento");

                foreach (var servidor in servidores.Ftp)
                {
                    if (Convert.ToBoolean(servidor.ServicoDownloadAtivo))
                        await _processadorFtp.DownloadArquivosFtp(servidor);
                
                    if (Convert.ToBoolean(servidor.ServicoUploadAtivo))
                        await _processadorFtp.UploadArquivosFtp(servidor);
                }

                Running = false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                Log.Error(exception, exception.Message);
                Running = false;
            }
        }

        private FtpModel BuscaServidores()
        {
            FtpModel servidores;

            using StreamReader r = new StreamReader("FtpConfig.json");
            
            var json = r.ReadToEnd();
                
            servidores =JsonConvert.DeserializeObject<FtpModel>(json);

            return servidores;  
        }
    }
}