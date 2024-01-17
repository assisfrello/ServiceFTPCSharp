# ServiceFTPCSharp
Serviço de FTP desenvolvido em C#, podendo ser habilitado diversos FTP's

No arquivo FTP Config deve ser informado os dados para operação do serviço.

No arquivo "FTPConfig.json", quando informada a tag "ServicoDownloadAtivo" igual a "true", não deve ser habilitada a "ServicoUploadAtivo", pois irá conflitar os diretórios de "Upload" e "Download" informados.

A tag "Servidor" deve ser preenchido com os dados do servidor onde será realizado o FTP.

A tag "Subdiretorio" serve para o usuário informar se o arquivo está dentro de uma pasta específica no servidor informado.
