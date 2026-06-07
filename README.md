# 🌩️ Sentinel Alert — Cloud/DevOps

> Plataforma inteligente de alertas de desastres naturais — ambiente containerizado na Azure

**Grupo:** Meteo Solution  
**Disciplina:** DevOps Tools & Cloud Computing — FIAP 2TDS  
**RM do representante:** 562822

---

## 📋 Sobre o Projeto

O **Sentinel Alert** é uma solução B2B2C que utiliza dados de satélite e APIs públicas (NASA EONET, CEMADEN, OpenWeatherMap) para prever riscos de desastres naturais e enviar alertas personalizados a cidadãos e autoridades de defesa civil.

Este repositório contém a infraestrutura containerizada da API .NET, com dois containers Docker integrados rodando em uma VM na Azure.

---

## 🏗️ Arquitetura Macro

```
┌─────────────────────────────────────────────────────────────┐
│  Azure for Students  •  canadacentral                       │
│  Resource Group: rg-meteosolution-gs                        │
│                                                             │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  VM: vm-rm562822-meteosolution                        │  │
│  │  Ubuntu 22.04 LTS  •  Standard_B2als_v2              │  │
│  │  Docker 29.5  •  Docker Compose 2.5                  │  │
│  │                                                       │  │
│  │  ┌─── meteosolution-network (bridge) ──────────────┐  │  │
│  │  │                                                 │  │  │
│  │  │  ┌──────────────────┐   ┌───────────────────┐  │  │  │
│  │  │  │  rm562822-app    │   │   rm562822-db     │  │  │  │
│  │  │  │  .NET 10 API     │──▶│   PostgreSQL 16   │  │  │  │
│  │  │  │  porta 8080      │   │   porta 5445:5432 │  │  │  │
│  │  │  │  USER appuser    │   │   volume nomeado  │  │  │  │
│  │  │  └──────────────────┘   └───────────────────┘  │  │  │
│  │  └─────────────────────────────────────────────────┘  │  │
│  │                                                       │  │
│  │  NSG: porta 22 (SSH) • porta 8080 (API)              │  │
│  └───────────────────────────────────────────────────────┘  │
│                         │                                   │
│               IP Público: <IP_PUBLICO>                     │
└─────────────────────────────────────────────────────────────┘
         │
   Usuário / navegador
   http://<IP_PUBLICO>:8080/swagger
```

---

## 🛠️ Stack

| Camada | Tecnologia |
|---|---|
| API | .NET 10, ASP.NET Core Web API |
| ORM | Entity Framework Core 10 (Code First) |
| Banco | PostgreSQL 16 |
| Containerização | Docker, Docker Compose |
| Nuvem | Microsoft Azure (VM Ubuntu 22.04) |
| Infraestrutura | Azure CLI (deploy via script bash) |

---

## 📦 Estrutura do Repositório

```
meteo-solution-cloud/
├── MeteoSolution.API/
│   ├── Controllers/
│   │   └── DTOs/
│   ├── Data/
│   ├── Migrations/
│   ├── Models/
│   ├── Repositories/
│   ├── Dockerfile          ← multi-stage build, usuário não-root
│   ├── Program.cs          ← migrations automáticas no startup
│   └── appsettings.json
├── docker-compose.yml      ← dois containers + rede + volume
├── deploy-azure.sh         ← provisiona VM na Azure via CLI
├── destroy-azure.sh        ← remove todos os recursos da Azure
└── .gitignore
```

---

## 🐳 Containers

### Container da Aplicação — `rm562822-app`
- Imagem personalizada via Dockerfile multi-stage
- Usuário não-root: `appuser`
- Diretório de trabalho: `/app`
- Porta exposta: `8080`
- Variável de ambiente: `ASPNETCORE_ENVIRONMENT=Production`

### Container do Banco de Dados — `rm562822-db`
- Imagem: `postgres:16`
- Porta mapeada: `5445:5432`
- Volume nomeado: `meteosolution_data`
- Variáveis de ambiente: `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`

---

## ☁️ How To — Do clone ao ambiente rodando na nuvem

### Pré-requisitos

- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) instalado
- Conta Azure ativa (Azure for Students)
- Git Bash (Windows) ou terminal Linux/Mac

---

### Passo 1 — Clonar o repositório

```bash
git clone https://github.com/FIAP-2026-GS1/meteo-solution-cloud.git
cd meteo-solution-cloud
```

---

### Passo 2 — Provisionar a VM na Azure

```bash
chmod +x deploy-azure.sh
./deploy-azure.sh
```

O script executa automaticamente:
1. Autentica na Azure e define a subscription
2. Cria o Resource Group `rg-meteosolution-gs` em `canadacentral`
3. Cria a VM Ubuntu 22.04 (`Standard_B2als_v2`)
4. Abre as portas 8080 (API) e 22 (SSH) no NSG
5. Instala Docker e Docker Compose na VM
6. Exibe o IP público no terminal

Ao final, o terminal exibirá:

```
  IP Público:    <IP_PUBLICO>
  Usuário SSH:   azureuser
```

---

### Passo 3 — Conectar na VM via SSH

```bash
ssh azureuser@<IP_PUBLICO>
```

Na primeira conexão, confirme com `yes` quando perguntado sobre o fingerprint.

---

### Passo 4 — Clonar e subir os containers na VM

Dentro da VM:

```bash
git clone https://github.com/FIAP-2026-GS1/meteo-solution-cloud.git
cd meteo-solution-cloud
docker compose up -d
```

O Docker irá:
- Baixar a imagem `postgres:16`
- Construir a imagem da API a partir do Dockerfile
- Criar a rede `meteosolution-network`
- Criar o volume `meteosolution_data`
- Subir os dois containers em background

---

### Passo 5 — Verificar os containers

```bash
docker ps
docker logs rm562822-db --tail 20
docker logs rm562822-app --tail 20
```

---

### Passo 6 — Validar usuário e estrutura de diretórios

```bash
docker exec rm562822-app whoami      # deve retornar: appuser
docker exec rm562822-app pwd         # deve retornar: /app
docker exec rm562822-app ls -l
docker exec rm562822-db whoami
```

---

### Passo 7 — Verificar persistência no banco

```bash
docker exec -it rm562822-db psql -U postgres -d meteosolution_db
```

Dentro do psql:

```sql
\dt
SELECT * FROM pais;
SELECT * FROM estado;
\q
```

---

### Passo 8 — Acessar o Swagger

Abra no navegador:

```
http://<IP_PUBLICO>:8080/swagger
```

A API expõe 20 endpoints CRUD para as entidades:
- `Pais` → `Estado` → `Cidade` → `RegiaoMonitorada`

---

### Passo 9 — Destruir o ambiente (evitar gastos)

```bash
chmod +x destroy-azure.sh
./destroy-azure.sh
```

Digite `sim` para confirmar. Todos os recursos do Resource Group serão removidos.

---

## 🔌 Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/Pais` | Listar países |
| POST | `/api/Pais` | Criar país |
| GET | `/api/Pais/{id}` | Buscar país por ID |
| PUT | `/api/Pais/{id}` | Atualizar país |
| DELETE | `/api/Pais/{id}` | Remover país |
| GET | `/api/Estado` | Listar estados |
| POST | `/api/Estado` | Criar estado |
| GET | `/api/Estado/{id}` | Buscar estado por ID |
| PUT | `/api/Estado/{id}` | Atualizar estado |
| DELETE | `/api/Estado/{id}` | Remover estado |
| GET | `/api/Cidade` | Listar cidades |
| POST | `/api/Cidade` | Criar cidade |
| GET | `/api/Cidade/{id}` | Buscar cidade por ID |
| PUT | `/api/Cidade/{id}` | Atualizar cidade |
| DELETE | `/api/Cidade/{id}` | Remover cidade |
| GET | `/api/RegiaoMonitorada` | Listar regiões |
| POST | `/api/RegiaoMonitorada` | Criar região |
| GET | `/api/RegiaoMonitorada/{id}` | Buscar região por ID |
| PUT | `/api/RegiaoMonitorada/{id}` | Atualizar região |
| DELETE | `/api/RegiaoMonitorada/{id}` | Remover região |

---

## 📌 Decisões Técnicas

- **Dockerfile multi-stage:** stage de build separado do runtime — imagem final menor e sem SDK
- **Usuário não-root:** `useradd -m appuser` + `USER appuser` por segurança
- **Migrations automáticas:** `db.Database.Migrate()` no startup — sem necessidade de intervenção manual
- **`DeleteBehavior.Restrict`:** integridade referencial garantida no banco
- **`ReferenceHandler.IgnoreCycles`:** evita ciclos de referência circular na serialização JSON
- **Volume nomeado:** persistência dos dados do PostgreSQL sobrevive ao `docker compose down`
- **`depends_on` com healthcheck:** o container app só sobe após o banco estar pronto

---

## 👥 Integrantes

| Nome | RM |
|---|---|
| Matt | 562822 |
