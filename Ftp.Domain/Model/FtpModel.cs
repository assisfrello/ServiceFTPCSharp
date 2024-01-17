using System.Collections.Generic;

namespace Ftp.Domain.Model
{
    public class FtpModel
    {
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<FtpDados> Ftp { get; set; }
    }
    
    public class ListaFtps
    {
        public List<FtpDados> Ftp { get; set; }
    }

    public class FtpDados
    {
        public string ServicoDownloadAtivo { get; set; }
        public string ServicoUploadAtivo { get; set; }
        public string Servidor { get; set; }
        public string Porta { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string SubdiretorioDownload { get; set; }
        public string SubdiretorioUpload { get; set; }
        public string DiretorioUpload { get; set; }
        public string DiretorioUploadEnviados { get; set; }
        public string DiretorioUploadErros { get; set; }
        public string DiretorioDownload { get; set; }
        public string DiretorioDownloadLidos { get; set; }
    }
}