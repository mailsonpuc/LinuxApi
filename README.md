# Distro API

API para gerenciamento de distribuições Linux (Distro). Projeto FullStack — neste repositório está a API em .NET (C#) responsável por categorias e distros, autenticação via JWT, persistência com Entity Framework Core e Identity, e documentação automática via NSwag/Swagger.




---


<video width="600" controls>
  <source src="video_2026-08-27_18-27-56.mp4" type="video/mp4">
  Seu navegador não suporta vídeos.
</video>


## Sumário

- Sobre
- Tecnologias
- Estrutura do repositório
- Pré-requisitos
- Configuração (appsettings)
- Banco de dados e migrações
- Executando a API
- Autenticação (JWT)
- Endpoints (resumo)
- Funcionalidade de IA / Ollama
- Exemplos de uso (curl)
- Swagger / Documentação interativa
- Contribuição
- Contato
- Licença

---

## Sobre

Esta API expõe operações CRUD para:
- Categorias (Category)
- Distribuições (Distro)

Além disso, fornece endpoints de autenticação (registro, login) com ASP.NET Identity e emissão de tokens JWT. Alguns endpoints requerem autenticação (atributo [Authorize]).
- Endpoints de geração de IA com integração Ollama para perguntas sobre Linux.

---

## Tecnologias

- .NET 10 (projetos em C#)
- ASP.NET Core Web API
- ASP.NET Identity (UserManager / SignInManager)
- Entity Framework Core (SQL Server)
- NSwag (Swagger) para documentação
- Migrations EF Core para schema inicial
- Integração com Ollama para geração de respostas baseadas em prompt
- Rate limiting para proteção de endpoints críticos

---

## Estrutura do repositório (principais pastas)

- DistroBackEnd/Distro.API — projeto da API (controllers, Program.cs, configuração)
- DistroBackEnd/Distro.API/Controllers/GenerateController.cs — endpoint de IA para geração de respostas
- DistroBackEnd/Distro.Application — DTOs e interfaces de aplicação (serviços)
- DistroBackEnd/Distro.Application/Services/OllamaService.cs — serviço que consome a API do Ollama
- DistroBackEnd/Distro.Domain — contratos e entidades de domínio
- DistroBackEnd/Distro.Infra.Data — contexto EF Core, migrations, Identity
- DistroBackEnd/Distro.Infra.IoC — injeção de dependenciancia, configurações (Swagger, JWT)
- DistroBackEnd/Distro.Infra.Data/Migrations — migrations EF Core (contém a migration `Inicial`)

---

## Pré-requisitos

- .NET SDK (versão compatível com o projeto; verifique global.json ou csproj)
- SQL Server (ou altere o provedor no DbContext se preferir outro SGDB)
- dotnet-ef (opcional, para aplicar/criar migrations): `dotnet tool install --global dotnet-ef`

---

## Configuração (appsettings)

A API espera que algumas chaves estejam configuradas no `appsettings.json` (ou em variáveis de ambiente). Os nomes abaixo são os usados no código:

- ConnectionStrings:DefaultConnection — string de conexão para o SQL Server
- Configuração de JWT — este projeto tem um método de extensão `AddJwtConfiguration`. Tipicamente as chaves incluem:
  - Jwt:Key (chave secreta)
  - Jwt:Issuer
  - Jwt:Audience
  - Jwt:ExpiryMinutes (opcional)
- Ollama — URL do serviço de geração de IA para o endpoint `/api/generate`.

Exemplo mínimo (appsettings.json):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DistroDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "SUA_CHAVE_SECRETA_MUITO_SEGURA_AQUI",
    "Issuer": "DistroApi",
    "Audience": "DistroApiUsers",
    "ExpiryMinutes": 60
  },
  "Ollama": {
    "Url": "http://localhost:11434/api/generate"
  }
}
```

Observação: verifique a implementação de `AddJwtConfiguration` no projeto `Distro.Infra.IoC` para os nomes exatos de configuração utilizados.

---

## Banco de dados e migrations

A migration inicial (`Inicial`) está presente no projeto `Distro.Infra.Data`. Para aplicar as migrations e criar o banco:

1. A partir da raiz do repositório (ou ajustando paths):

- Aplicar migrations via CLI:

```bash
# a partir da raiz do repo
dotnet ef database update --project DistroBackEnd/Distro.Infra.Data --startup-project DistroBackEnd/Distro.API
```

- Caso queira criar novas migrations:
```bash
dotnet ef migrations add NomeDaMigration --project DistroBackEnd/Distro.Infra.Data --startup-project DistroBackEnd/Distro.API
```

Certifique-se de que a string de conexão `DefaultConnection` aponta para um servidor SQL acessível.

---

## Executando a API

1. Restaurar dependências e compilar:

```bash
dotnet restore
dotnet build
```

2. Executar a API (exemplo executando o projeto Distro.API):

```bash
cd DistroBackEnd/Distro.API
dotnet run
```

Em ambiente de desenvolvimento, o projeto ativa o Swagger UI (`UseOpenApi()` e `UseSwaggerUi()` no `Program.cs`) para facilitar testes.

---

## Autenticação (JWT)

- Endpoints de autenticação:
  - POST /api/token/register — registrar novo usuário (email + senha)
  - POST /api/token/login — autenticar e obter token JWT

Após o login, a API retorna um objeto `UserToken` com:
- Token (string JWT)
- Expiration (DateTime)

Use esse token no cabeçalho Authorization para acessar endpoints protegidos:

Header:
```
Authorization: Bearer <token>
```

### Validação de registro

- O endpoint de registro verifica se o e-mail já está cadastrado.
- Quando o e-mail já existe, retorna `400 BadRequest` com o texto `e-mail ja registrado`.

---

## Endpoints (resumo)

- TokenController (sem autenticação necessária):
  - POST /api/token/register
    - Request: RegisterModels { Email, Password, ConfirmPassword }
    - Retorna 200 OK com mensagem de sucesso ou 400 BadRequest com erro
  - POST /api/token/login
    - Request: LoginModels { Email, Password }
    - Retorna 200 OK => UserToken { Token, Expiration } ou 401 Unauthorized

- GenerateController:
  - POST /api/generate
    - Requer JSON com `model` e `prompt`
    - O prompt deve conter a palavra "linux" para ser válido
    - Retorna 200 OK => GenerateResponseDTO { Answer }
    - Rate limit: 3 requisições a cada 10 minutos

- CategoryController:
  - GET /api/category
    - Retorna lista de categorias (paginada)
    - Parâmetros opcionais: pageNumber (padrão 1), pageSize (padrão 10)
  - GET /api/category/{id}
    - Retorna categoria por ID
  - POST /api/category
    - Requer autenticação
    - Cria nova categoria
  - PUT /api/category/{id}
    - Requer autenticação
    - Atualiza categoria
  - DELETE /api/category/{id}
    - Requer autenticação
    - Remove categoria

- DistroController:
  - GET /api/distro
    - Retorna lista de distros (paginada)
    - Parâmetros opcionais: pageNumber (padrão 1), pageSize (padrão 10)
  - GET /api/distro/{id}
    - Retorna distro por ID
  - POST /api/distro
    - Cria nova distro
  - PUT /api/distro/{id}
    - Atualiza distro
  - DELETE /api/distro/{id}
    - Remove distro

### Funcionalidade de IA / Ollama

- Endpoint para geração de respostas com prompts sobre Linux
- Validação do prompt exige a palavra "linux"
- Serviço `OllamaService` consome a API do Ollama via `IAIService`
- Regras de rate limiting:
  - Política `generate` permite 3 requisições a cada 10 minutos

### DTOs principais

- CategoryDTO: `CategoryId`, `Name`
- DistroDTO: `DistroId`, `ImageUrl`, `Nome`, `Descricao`, `Iso`, `CategoryId`

Os DTOs estão no projeto `Distro.Application.DTOs`.

---

## Exemplos de requisições

Registrar usuário:

```bash
curl -X POST "https://localhost:5001/api/token/register" \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"SenhaSegura123!"}'
```

Login e obtenção de token:

```bash
curl -X POST "https://localhost:5001/api/token/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"SenhaSegura123!"}'
```

Resposta de sucesso (exemplo):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2026-02-27T12:34:56Z"
}
```

Usar token nos endpoints protegidos:

```bash
curl -X GET "https://localhost:5001/api/category" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

Obter categorias com paginação (página 2, 15 itens por página):

```bash
curl -X GET "https://localhost:5001/api/category?pageNumber=2&pageSize=15" \
  -H "Authorization: Bearer <token>"
```

Obter distros com paginação (página 1, 20 itens):

```bash
curl -X GET "https://localhost:5001/api/distro?pageNumber=1&pageSize=20"
```

Criar categoria (exemplo):

```bash
curl -X POST "https://localhost:5001/api/category" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{"name":"Desenvolvimento"}'
```

Criar distro (exemplo):

```bash
curl -X POST "https://localhost:5001/api/distro" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "nome":"MinhaDistro",
    "descricao":"Distro custom",
    "iso":"minha-distro.iso",
    "imageUrl":"https://exemplo.com/imagem.png",
    "categoryId":"89012345-6789-4678-9012-bcdefabcdefa"
  }'
```

Exemplo de uso do endpoint de IA:

```bash
curl -X POST "https://localhost:5001/api/generate" \
  -H "Content-Type: application/json" \
  -d '{
    "model":"llama2",
    "prompt":"Escreva um breve resumo sobre Linux e suas vantagens para desenvolvedores."
  }'
```

(Ajuste os campos conforme os DTOs presentes no projeto.)

---

## Paginação

Os endpoints `GET /api/category` e `GET /api/distro` suportam paginação por parâmetros de query string:

- `pageNumber` (padrão: 1) — número da página
- `pageSize` (padrão: 10) — quantidade de itens por página

Exemplo de requisição:

```
GET /api/category?pageNumber=2&pageSize=15
```

Resposta:

O retorno é um objeto `PagedList<T>` com os seguintes campos:

- `items` — array com os elementos da página
- `currentPage` — página atual
- `totalPages` — total de páginas
- `pageSize` — quantidade de itens por página
- `totalCount` — total de elementos
- `hasPreviousPage` — se existe página anterior
- `hasNextPage` — se existe próxima página

---

## Regras adicionais

- Rate limiting está ativado por controller com a política `fixedwindow`.
- A política atual permite 100 requisições a cada 5 segundos
- O endpoint `/api/generate` usa a política `generate` com 3 requisições a cada 10 minutos
- Requisições acima do limite retornam `429 Too Many Requests`.
- CORS está configurado com a política `AllowAll`.
- Registro de usuário com e-mail duplicado retorna `400 BadRequest` e a mensagem `e-mail ja registrado`.

---

## Swagger / Documentação interativa

Quando a API é executada em ambiente de desenvolvimento, o NSwag expõe a documentação e UI do Swagger. Acesse algo como:

- https://localhost:5001/swagger
- ou a rota base /swagger (verifique portas e launchSettings)

Lá é possível testar os endpoints e preencher o campo Authorization (Bearer token).

---

## Observações técnicas e pontos importantes

- A injeção de dependência e configurações estão centralizadas em `Distro.Infra.IoC` (métodos `AddInfrastructureIoC`, `AddInfrastructureSwagger`, `AddJwtConfiguration`).
- O serviço de IA `OllamaService` implementa `IAIService` e consome o endpoint configurado em `Ollama:Url`.
- O controller `GenerateController` valida prompts e retorna respostas geradas pela API do Ollama.
- Autenticação usa `AuthenticateService` (implementa `IAuthenticate`) que utiliza `UserManager<ApplicationUser>` e `SignInManager<ApplicationUser>`.
- Verifique os DTOs e validações (ModelState) nos Controllers para os requisitos dos objetos enviados.
- A migration `Inicial` já cria as tabelas `Categories` e `Distros` e insere alguns dados seed (veja a migration em `Distro.Infra.Data/Migrations`).

---

## Como contribuir

1. Abra uma issue descrevendo a mudança desejada.
2. Faça um fork e crie uma branch com um nome descritivo (`feat/nova-funcao`, `fix/corrige-bug`).
3. Faça commits pequenos e claros.
4. Abra um Pull Request explicando o que foi implementado e por quê.

---

## Contato

- Autor / Contato (conforme info em Swagger): [mailsonpuc](https://github.com/mailsonpuc)  
- E-mail (exemplo usado no projeto): mailson.costa@protonmail.com

---
