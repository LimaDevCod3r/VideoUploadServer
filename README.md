# 🎬 VideoUploadServer

API REST desenvolvida em **ASP.NET Core** para upload, listagem e gerenciamento de vídeos via rede local. Projetada para receber vídeos de um aplicativo Android (Kotlin) via WiFi e salvá-los no computador.

---

## 🚀 Tecnologias

- **ASP.NET Core** (.NET 10)
- **FFMpegCore** — leitura de metadados de vídeo (resolução)
- **FFmpeg/FFProbe** — análise de arquivos de mídia
- **User Secrets** — armazenamento seguro de credenciais

---

## 📁 Estrutura do Projeto

```
VideoUploadServer/
├── Controllers/
│   └── UploadController.cs       # Endpoints HTTP (GET, POST, DELETE)
├── Services/
│   └── UploadService.cs          # Lógica de negócio (salvar, listar, deletar)
├── Middlewares/
│   └── ApiKeyMiddleware.cs       # Autenticação por API Key
├── Dtos/
│   └── Responses/
│       ├── UploadResult.cs       # DTO de resposta do upload
│       └── VideoInfo.cs          # DTO de resposta da listagem
├── Program.cs                    # Configuração da aplicação
└── appsettings.json              # Configurações gerais
```

---

## 🌐 Endpoints

| Método   | Rota                     | Descrição                              | Resposta                 |
| -------- | ------------------------ | -------------------------------------- | ------------------------ |
| `GET`    | `/api/upload`            | Lista todos os vídeos com resolução    | `200 OK`                 |
| `GET`    | `/api/upload/{fileName}` | Busca um vídeo pelo nome               | `200 OK` / `404`         |
| `POST`   | `/api/upload`            | Upload de vídeos (multipart/form-data) | `201 Created`            |
| `DELETE` | `/api/upload/{fileName}` | Deleta um vídeo pelo nome              | `204 No Content` / `404` |

### 🔐 Autenticação

Todos os endpoints exigem o header:

```
x-api-key: <sua-chave-secreta>
```

---

## ⚙️ Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [FFmpeg](https://www.gyan.dev/ffmpeg/builds/) instalado em `C:\ffmpeg\bin`

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/VideoUploadServer.git
cd VideoUploadServer
```

### 2. Configure o User Secrets (API Key)

```bash
dotnet user-secrets init
dotnet user-secrets set "ApiSecurity:Apikey" "sua-chave-secreta"
```

### 3. Execute

```bash
dotnet run
```

O servidor estará disponível em `http://0.0.0.0:5233`

---

## 📱 Uso com Android

Para conectar um app Android ao servidor:

1. Certifique-se de que o PC e o celular estão na **mesma rede WiFi**
2. Descubra o IP do PC com `ipconfig` (procure pelo IPv4 do adaptador Ethernet/WiFi)
3. Libere a porta `5233` no **Firewall do Windows**:

```powershell
New-NetFirewallRule -DisplayName "VideoUploadServer API" -Direction Inbound -Protocol TCP -LocalPort 5233 -Action Allow
```

4. No app Android, use a URL:

```
http://<IP-DO-PC>:5233/api/upload
```

---

## 📋 Exemplos de Requisição

### Upload de vídeo

```bash
curl -X POST http://localhost:5233/api/upload \
  -H "x-api-key: sua-chave-secreta" \
  -F "videos=@video.mp4"
```

### Listar todos os vídeos

```bash
curl http://localhost:5233/api/upload \
  -H "x-api-key: sua-chave-secreta"
```

**Resposta:**

```json
[
  {
    "name": "video.mp4",
    "length": 77855251,
    "resolution": "1920x1080",
    "extension": ".mp4",
    "creationTimeUtc": "2026-03-02T07:55:49Z"
  }
]
```

### Deletar um vídeo

```bash
curl -X DELETE http://localhost:5233/api/upload/video.mp4 \
  -H "x-api-key: sua-chave-secreta"
```

---

## 🛡️ Segurança

- A API Key é armazenada via **User Secrets**, fora do código-fonte
- Nunca versione credenciais no repositório
- O middleware intercepta **todas** as requisições antes de chegar nos controllers

---

## 📄 Licença

Este projeto é de uso pessoal.
