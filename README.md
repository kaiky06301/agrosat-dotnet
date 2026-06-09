# AgroSat — API .NET (ASP.NET Core + EF Core)

Disciplina **.NET** da Global Solution FIAP 2026/1. Esta API expõe um CRUD
REST completo para o domínio **AgroSat** (agricultura de precisão: cruza dados
de satélite — NDVI, umidade estimada, chuva — com sensores ESP32 no campo para
acionar irrigação e alertas). ODS 2, 8, 9, 13.

> Os nomes de entidade, campo e endpoint seguem **exatamente** o
> `CONTRATO-DOMINIO.md` e o esquema Oracle em `database/01_ddl.sql`, para
> conversar com as outras disciplinas (Oracle, API Java, Mobile, IoT).

## 👥 Integrantes (Grupo 4 — Turma 2TDSR)
- RM 565733 — Erick Bernardes Bradaschia
- RM 564054 — Gabriel Santos Claudino
- RM 565060 — Jonathan Moreira Gomes
- RM 566067 — Kaiky de Oliveira Silva
- RM 559523 — Lucas Fortes de Lima

## 🔗 Links da entrega
- 🎥 **Vídeo de demonstração (YouTube):** https://youtu.be/_CTZjjxcHHg
- 🎤 **Vídeo Pitch (YouTube):** _COLE_AQUI_O_LINK_
- 💻 **Repositório:** https://github.com/kaiky06301/agrosat-dotnet

## Stack
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core 8
- Provider **Oracle** (`Oracle.EntityFrameworkCore`) para produção/FIAP
- Swagger (Swashbuckle) habilitado
- Namespace base: `AgroSat.Api`

## Estrutura do projeto
```
api-dotnet/
├── AgroSat.Api.csproj
├── Program.cs                 # bootstrap, DI, EF, Swagger
├── appsettings.json           # connection string Oracle (placeholder)
├── Models/                    # 10 entidades do contrato
├── Data/AppDbContext.cs       # DbContext + relacionamentos (navegação EF)
├── Controllers/               # 10 controllers REST (CRUD completo)
└── Migrations/                # InitialCreate (exemplo) + ModelSnapshot
```

## Entidades (10) e relacionamentos
Usuario, Propriedade, Talhao, Cultura, Sensor, LeituraSensor, DadoSatelite,
AlertaAgricola, Recomendacao, Irrigacao.

Relacionamentos 1:N (navegação EF configurada no `AppDbContext`):
`Usuario→Propriedade→Talhao`; `Cultura→Talhao`;
`Talhao→Sensor→LeituraSensor`; `Talhao→DadoSatelite`;
`Talhao→AlertaAgricola→Recomendacao`; `Talhao→Irrigacao`.

## Endpoints REST (base `/api`)
Cada recurso expõe `GET /`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}`:

| Recurso          | Rota                  |
|------------------|-----------------------|
| Usuários         | `/api/usuarios`       |
| Propriedades     | `/api/propriedades`   |
| Talhões          | `/api/talhoes`        |
| Culturas         | `/api/culturas`       |
| Sensores         | `/api/sensores`       |
| Leituras         | `/api/leituras`       |
| Dados de satélite| `/api/dados-satelite` |
| Alertas          | `/api/alertas`        |
| Recomendações    | `/api/recomendacoes`  |
| Irrigações       | `/api/irrigacoes`     |

JSON em camelCase, datas em ISO-8601.

---

## Como rodar

### 1. Pré-requisitos
- .NET SDK 8 instalado (`dotnet --version` deve mostrar 8.x)
- Ferramenta EF Core CLI:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### 2. Restaurar pacotes
```bash
cd api-dotnet
dotnet restore
```

### 3. Configurar a connection string Oracle
Edite `appsettings.json` → `ConnectionStrings:OracleConnection` com **suas
credenciais da FIAP** (RM e senha):
```json
"OracleConnection": "User Id=RM999999;Password=SUASENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
```

### 4. Migration (gerar e aplicar)
A pasta `Migrations/` já traz uma **`InitialCreate` de exemplo** espelhando o
DDL. **Recomendado:** regenerar para o EF sincronizar 100% com o provider
Oracle instalado na sua máquina:
```bash
# remove a migration de exemplo e gera uma limpa
dotnet ef migrations remove
dotnet ef migrations add InitialCreate

# cria/atualiza as tabelas no Oracle
dotnet ef database update
```
> Se as tabelas **já existem** no Oracle (você rodou o `01_ddl.sql`), NÃO rode
> `database update` — só use a API direto. As tabelas/colunas batem.

### 5. Subir a API
```bash
dotnet run
```
Acesse o **Swagger**: `https://localhost:7099/swagger`
(a raiz `/` redireciona para o Swagger).

---

## Teste local SEM Oracle (opcional)
Para testar sem credenciais Oracle:
1. No `AgroSat.Api.csproj`, descomente o `PackageReference` do
   `Microsoft.EntityFrameworkCore.InMemory` (ou `...Sqlite`).
2. No `Program.cs`, comente o bloco `options.UseOracle(...)` e descomente o
   bloco InMemory (ou Sqlite) logo abaixo (há comentários guiando).
3. `dotnet run`. O InMemory não precisa de migration.

---

## Comandos úteis
```bash
dotnet build                       # compilar
dotnet run                         # subir API + Swagger
dotnet ef migrations add <Nome>    # nova migration
dotnet ef database update          # aplicar migrations no banco
dotnet ef migrations list          # listar migrations
```
