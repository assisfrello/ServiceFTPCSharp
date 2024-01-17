using System.IO;

namespace Ftp.Domain
{
    public static class ManipuladorArquivos
    {
        public static void MoverParaDiretorioLido(FileInfo file, string pathArquivoLeitura, string pathArquivoLido)
        {
            var directoryInfo = new DirectoryInfo(pathArquivoLeitura ?? string.Empty);
            
            if (!directoryInfo.Exists)
                return;

            if (File.Exists(@$"{pathArquivoLido}\{file.Name}"))
                File.Delete(@$"{pathArquivoLido}\{file.Name}");
            
            File.Move(@$"{pathArquivoLeitura}\{file.Name}", @$"{pathArquivoLido}\{file.Name}");
        }
        
        public static void MoverParaDiretorioErro(FileInfo file, string pathArquivoLeitura, string pathArquivoErro)
        {
            var directoryInfo = new DirectoryInfo(pathArquivoErro ?? string.Empty);

            if (!directoryInfo.Exists) 
                return;

            if (File.Exists(@$"{pathArquivoErro}\{file.Name}"))
                File.Delete(@$"{pathArquivoErro}\{file.Name}");
            
            File.Move(@$"{pathArquivoLeitura}\{file.Name}", @$"{pathArquivoErro}\{file.Name}", false);
        }
    }
}