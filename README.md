# 🌱 SunHarvest Agro — Irrigação Solar Inteligente
 
> "O sol irriga sua lavoura — nós garantimos que nenhuma gota seja desperdiçada e nenhuma bomba pare sem você saber."
 
API RESTful desenvolvida em ASP.NET Core para gerenciamento de fazendas, monitoramento de bombas solares e alertas inteligentes de irrigação. O sistema permite que produtores rurais acompanhem o desempenho de seus sistemas de irrigação solar e recebam alertas quando algo sair do esperado.
 
---
 
## 🧠 Sobre o Projeto
 
O Brasil é o maior irrigante da América Latina. Mais de 8,2 milhões de hectares são irrigados, consumindo ~70% da água doce retirada no país. O SunHarvest Agro resolve dois problemas centrais:
 
- *Irrigação por achismo* — sem dados, o produtor irriga por calendário fixo, desperdiçando até 30% a mais de água
- *Bomba solar invisível* — falhas em áreas remotas só são descobertas quando a lavoura já está murchando
 
---
 
## 🛠️ Tecnologias Utilizadas
 
| Tecnologia | Versão | Uso |
|------------|--------|-----|
| ASP.NET Core | 10 | Framework da API |
| Entity Framework Core | 10.0.8 | ORM |
| Oracle Database | 19c | Banco de dados |
| Oracle.EntityFrameworkCore | 10.23.x | Driver Oracle |
| Scalar.AspNetCore | 2.x | Documentação interativa |
| C# | 13 | Linguagem |
 
--- 

## 📁 Arquitetura e Organização do Projeto

```text
SunHarvestApiGS/
├── Controllers/
│   ├── UsuariosController.cs       → endpoints de usuários
│   ├── FazendasController.cs       → endpoints de fazendas
│   └── AlertasController.cs        → endpoints de alertas
├── Data/
│   └── AppDbContext.cs             → contexto do banco de dados
├── Models/
│   ├── Usuario.cs                  → entidade usuário
│   ├── Fazenda.cs                  → entidade fazenda
│   ├── Alerta.cs                   → entidade alerta
│   ├── UsuarioRequest.cs           → DTO de entrada usuário
│   ├── FazendaRequest.cs           → DTO de entrada fazenda
│   └── AlertaRequest.cs            → DTO de entrada alerta
├── Migrations/                     → histórico de mudanças no banco
├── appsettings.json                → configuração da connection string
└── Program.cs                      → configuração da aplicação
```
 
## 🗄️ Diagrama do Banco de Dados

```text
TB_USUARIO
├── Id (PK)
├── Nome
├── Email (UNIQUE)
└── Senha

      1 ─────── N

TB_FAZENDA
├── Id (PK)
├── Nome
├── Latitude
├── Longitude
├── Altitude
├── TipoCultura
├── TipoSolo
├── Area
├── Irrigacao
├── CapacidadePainel
├── PotenciaBomba
├── AzimutGraus
├── TaxaDesempenho
├── IdDispositivoIot (UNIQUE)
└── UsuarioId (FK)

      1 ─────── N

TB_ALERTA
├── Id (PK)
├── Mensagem
├── Severidade
├── Confirmado
├── DataCriacao
└── FazendaId (FK)
```
Ao excluir um usuário:
- todas as fazendas dele são removidas automaticamente
- todos os alertas das fazendas também são removidos

Ao excluir uma fazenda:
- todos os alertas vinculados são removidos automaticamente



 
## ⚙️ Instalação e Execução
 
### Pré-requisitos
 
- Visual Studio 2022 ou superior
- .NET 10 SDK
- Acesso ao Oracle Database (FIAP)
- dotnet-ef instalado globalmente
 
### Passo a passo
 
*1. Clone o repositório*
bash
git clone https://github.com/Laura853/SunHarvestAgro.Net.git
cd SunHarvestApiGS

 
*2. Configure a connection string*
 
Abra appsettings.json e preencha com suas credenciais:
json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}

 
*3. Instale os pacotes NuGet*
powershell
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Oracle.EntityFrameworkCore
Install-Package Scalar.AspNetCore

 
*4. Instale o dotnet-ef globalmente*
bash
dotnet tool install --global dotnet-ef

 
*5. Crie as tabelas no banco*
bash
dotnet ef migrations add InitialCreate
dotnet ef database update

 
*6. Execute o projeto*
 
Pressione F5 no Visual Studio. O Scalar abrirá automaticamente em:

https://localhost:{porta}/scalar/v1

 
---
 
## 📋 Documentação das Rotas
 
### 👤 Usuarios — /api/v1/usuarios
 
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/v1/usuarios | Lista todos os usuários |
| GET | /api/v1/usuarios/{id} | Busca usuário por ID |
| GET | /api/v1/usuarios/email/{email} | Busca usuário por email |
| POST | /api/v1/usuarios | Cadastra novo usuário |
| PUT | /api/v1/usuarios/{id} | Atualiza dados do usuário |
| DELETE | /api/v1/usuarios/{id} | Remove usuário |
 
### 🌾 Fazendas — /api/v1/fazendas
 
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/v1/fazendas | Lista todas as fazendas |
| GET | /api/v1/fazendas/{id} | Busca fazenda por ID |
| GET | /api/v1/fazendas/usuario/{idUsuario} | Lista fazendas de um usuário |
| POST | /api/v1/fazendas | Cadastra nova fazenda |
| PUT | /api/v1/fazendas/{id} | Atualiza dados da fazenda |
| DELETE | /api/v1/fazendas/{id} | Remove fazenda |
 
### 🚨 Alertas — /api/v1/alertas
 
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/v1/alertas/{id} | Busca alerta por ID |
| GET | /api/v1/alertas/fazenda/{idFazenda}?severidade=alto | Lista alertas de uma fazenda com filtros |
| POST | /api/v1/alertas | Cadastra novo alerta |
| PUT | /api/v1/alertas/{id} | Atualiza alerta |
| DELETE | /api/v1/alertas/{id} | Remove alerta |
 
---
 
## 🧪 Exemplos de Testes
 
### Ordem recomendada para testar
 
Sempre siga essa ordem pois as tabelas têm dependências:
1. POST /api/v1/usuarios
2. POST /api/v1/fazendas (com UsuarioId retornado)
3. POST /api/v1/alertas (com FazendaId retornado)
 
---
 
### 👤 Testes de Usuário
 
*POST /api/v1/usuarios — Cadastrar usuário*
json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "senha123"
}

Retorno esperado: 201 Created
json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com"
}

 
*GET /api/v1/usuarios/1 — Buscar por ID*
Retorno esperado: 200 OK com fazendas do usuário
 
*PUT /api/v1/usuarios/1 — Atualizar usuário*
json
{
  "nome": "João Silva Atualizado",
  "email": "joao.novo@email.com",
  "senha": "novasenha123"
}

Retorno esperado: 204 No Content
 
*DELETE /api/v1/usuarios/1*
Retorno esperado: 204 No Content
 
---
 
### 🌾 Testes de Fazenda
 
*POST /api/v1/fazendas — Cadastrar fazenda*
json
{
  "nome": "Fazenda São João",
  "latitude": -10.5,
  "longitude": -45.2,
  "altitude": 500,
  "tipoCultura": "Soja",
  "tipoSolo": "Franco",
  "area": 5,
  "irrigacao": 85,
  "capacidadePainel": 3000,
  "potenciaBomba": 1200,
  "azimutGraus": 180,
  "taxaDesempenho": 85.5,
  "idDispositivoIot": "IOT-001",
  "usuarioId": 1
}

Retorno esperado: 201 Created
 
*GET /api/v1/fazendas/usuario/1 — Fazendas de um usuário*
Retorno esperado: 200 OK com lista de fazendas do usuário 1
 
---
 
### 🚨 Testes de Alerta
 
*POST /api/v1/alertas — Cadastrar alerta*
json
{
  "mensagem": "Bomba operando a 40% da vazão esperada — painel pode estar sujo",
  "severidade": "alto",
  "confirmado": false,
  "fazendaId": 1
}

Retorno esperado: 201 Created
 
*GET /api/v1/alertas/fazenda/1  — Alertas de uma fazenda* 
Retorno esperado: 200 OK com alertas
 
*PUT /api/v1/alertas/1 — Reconhecer alerta*
json
{
  "mensagem": "Bomba operando a 40% da vazão esperada — painel pode estar sujo",
  "severidade": "alto",
  "confirmado": true,
  "fazendaId": 1
}

Retorno esperado: 204 No Content
 
---
 
### ❌ Testes de Validação (entradas inválidas)
 
*Email inválido — retorna 400:*
json
{
  "nome": "João",
  "email": "emailinvalido",
  "senha": "123456"
}

 
*Senha curta — retorna 400:*
json
{
  "nome": "João",
  "email": "joao@email.com",
  "senha": "123"
}

 
*Severidade inválida — retorna 400:*
json
{
  "mensagem": "Alerta teste",
  "severidade": "urgente",
  "fazendaId": 1
}

 
*FazendaId inexistente — retorna 404:*
json
{
  "mensagem": "Alerta teste",
  "severidade": "alto",
  "fazendaId": 999
}

 
---
 
 
*CCGL* — Análise e Desenvolvimento de Sistemas — FIAP 2026
