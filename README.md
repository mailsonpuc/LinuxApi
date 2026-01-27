# Distro API

API para gerenciamento de distribuições Linux (Distro). Projeto FullStack — neste repositório está a API em .NET (C#) responsável por categorias e distros, autenticação via JWT, persistência com Entity Framework Core e Identity, e documentação automática via NSwag/Swagger.

> Descrição do repositório: API PARA DISTROS LINUX, projeto fullStack.

---

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

---

## Tecnologias

- .NET 10 (projetos em C#)
- ASP.NET Core Web API
- ASP.NET Identity (UserManager / SignInManager)
- Entity Framework Core (SQL Server)
- NSwag (Swagger) para documentação
- Migrations EF Core para schema inicial

---

## Estrutura do repositório (principais pastas)

- DistroBackEnd/Distro.API — projeto da API (controllers, Program.cs, configuração)
- DistroBackEnd/Distro.Application — DTOs e interfaces de aplicação (serviços)
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

Use esse token no header Authorization para acessar endpoints protegidos:

Header:
```
Authorization: Bearer <token>
```

---

## Endpoints (resumo)

- TokenController (sem autenticação necessária):
  - POST /api/token/register
    - Request: RegisterModels { Email, Password }
    - Response: 200 OK (mensagem) ou 400 BadRequest
  - POST /api/token/login
    - Request: LoginModels { Email, Password }
    - Response: 200 OK => UserToken { Token, Expiration } ou 401 Unauthorized

- CategoryController (protegido, exige Bearer token):
  - GET /api/category
    - Retorna lista de CategoryDTO
  - GET /api/category/{id}
    - Retorna CategoryDTO por id
  - POST /api/category
    - Cria nova categoria (CategoryDTO)
  - PUT /api/category/{id}
    - Atualiza categoria
  - DELETE /api/category/{id}
    - Remove categoria

- DistroController (protegido, exige Bearer token):
  - GET /api/distro
    - Retorna lista de DistroDTO
  - GET /api/distro/{id}
    - Retorna DistroDTO por id
  - POST /api/distro
    - Cria distro (DistroDTO)
  - PUT /api/distro/{id}
    - Atualiza distro
  - DELETE /api/distro/{id}
    - Remove distro

Observação: os DTOs (CategoryDTO, DistroDTO) estão no projeto Distro.Application.DTOs — ver a pasta para campos exatos (por exemplo: Distro tem DistroId, Nome, Descricao, Iso, ImageUrl, CategoryId, etc.)

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

(Ajuste os campos conforme os DTOs presentes no projeto.)

---

## Swagger / Documentação interativa

Quando a API é executada em ambiente de desenvolvimento, o NSwag expõe a documentação e UI do Swagger. Acesse algo como:

- https://localhost:5001/swagger
- ou a rota base /swagger (verifique portas e launchSettings)

Lá é possível testar os endpoints e preencher o campo Authorization (Bearer token).

---

## Observações técnicas e pontos importantes

- A injeção de dependência e configurações estão centralizadas em `Distro.Infra.IoC` (métodos `AddInfrastructureIoC`, `AddInfrastructureSwagger`, `AddJwtConfiguration`).
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

## Licença

A licença não está explicitada no repositório. Adicione um arquivo `LICENSE` com a licença desejada (MIT, Apache-2.0, etc.) se quiser deixar explícito o uso.

---

Se quiser, eu posso:
- abrir um Pull Request com este README.md no repositório,
- ou gerar um README mais detalhado incluindo exemplos reais dos DTOs (posso ler os arquivos DTO e gerar payloads exatos),
- ou adicionar instruções para implantação em Docker / Azure / AWS.

Diga qual opção prefere que eu faça a seguir.