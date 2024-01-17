using System;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using Ftp.CrossCutting.Email;
using Ftp.Domain.Model;
using Renci.SshNet;

namespace Ftp.Domain
{
    public class ProcessadorFtp
    {
        public async Task DownloadArquivosFtp(FtpDados servidor)
        {
            var host = servidor.Servidor;
            var porta = Convert.ToInt32(servidor.Porta);
            var usuario = servidor.Usuario;
            var senha = servidor.Senha;
            
            var ftpSubdiretorio = servidor.SubdiretorioDownload;
            var diretorioDownload = servidor.DiretorioDownload;
            var diretorioDownloadLidos = servidor.DiretorioDownloadLidos;
            
            if (ftpSubdiretorio == null || diretorioDownload == null) return;

            var diaLeitura = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            
            try
            {
                using var sftp = new SftpClient(host, porta, usuario, senha);
                
                sftp.Connect();

                var subdiretorio = string.IsNullOrEmpty(ftpSubdiretorio)
                    ? "//"
                    : ftpSubdiretorio;
                    
                var arquivosDiretorio = sftp.ListDirectory(subdiretorio);

                foreach (var fi in arquivosDiretorio)
                {
                    if (fi.IsDirectory || fi.Name.Length <= 2)
                        continue;

                    using var file = File.OpenWrite(diretorioDownload + fi.Name);

                    var arquivo = Path.Combine(ftpSubdiretorio, fi.Name);
                    
                    sftp.DownloadFile(arquivo, file);
                    
                    if (!string.IsNullOrEmpty(diretorioDownloadLidos))
                    {
                        using var fileBackup = File.OpenWrite(diretorioDownloadLidos + fi.Name);
                        sftp.DownloadFile(arquivo, fileBackup);
                    }
                    
                    sftp.DeleteFile(arquivo);
                }
                
                sftp.Disconnect();
            }
            catch (Exception e)
            {
                await EmailNotification.Send(e, e.Message, diaLeitura, "DownloadArquivosFtp");
            }
        }

        public async Task UploadArquivosFtp(FtpDados servidor)
        {
            var host = servidor.Servidor;
            var porta = Convert.ToInt32(servidor.Porta);
            var usuario = servidor.Usuario;
            var senha = servidor.Senha;
            
            var ftpSubdiretorio = servidor.SubdiretorioUpload;
            var caminhoExportacao = servidor.DiretorioUpload;
            var caminhoBackup = servidor.DiretorioUploadEnviados;

            if (caminhoExportacao == null || caminhoBackup == null) return;

            var arquivosDiretorio = Directory.GetFiles(caminhoExportacao);
            
            foreach (var fi in arquivosDiretorio)
            {
                var exportacaoArquivo = Path.Combine(caminhoExportacao, fi);
                var fileinfo = new FileInfo(exportacaoArquivo);

                try
                {
                    using var sftp = new SftpClient(host, porta, usuario, senha);
                    
                    sftp.Connect();
                        
                    if (!string.IsNullOrEmpty(ftpSubdiretorio))
                        sftp.ChangeDirectory(ftpSubdiretorio);

                    using (var fileStream = new FileStream(exportacaoArquivo, FileMode.Open))
                    {
                        sftp.BufferSize = 4 * 1024;
                        sftp.UploadFile(fileStream, Path.GetFileName(exportacaoArquivo));
                    }

                    sftp.Disconnect();
                
                    ManipuladorArquivos.MoverParaDiretorioLido(fileinfo, caminhoExportacao, servidor.DiretorioUploadEnviados);
                }
                catch (Exception e)
                {
                    await EmailNotification.Send(e, e.Message, fileinfo.Name, "UploadArquivosFtp");
                
                    ManipuladorArquivos.MoverParaDiretorioErro(fileinfo, caminhoExportacao, servidor.DiretorioUploadErros);
                }
            }
        }
    }
}