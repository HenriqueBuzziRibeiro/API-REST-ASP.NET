# API REST de Cadastro de Usuários

API REST simples de cadastro de usuários, construída em **ASP.NET Core 9**.

## Stack

- ASP.NET Core 9 (Web API, estilo Controllers)
- Entity Framework Core 9
- SQLite
- Swagger / Swashbuckle

## Endpoints

| Verbo | Rota | Descrição |
|---|---|---|
| `GET` | `/api/usuarios` | Lista usuários — aceita filtro por `?cargo=` e `?ativo=` |
| `GET` | `/api/usuarios/{id}` | Busca um usuário por id |
| `POST` | `/api/usuarios` | Cria um usuário |
| `PUT` | `/api/usuarios/{id}` | Atualiza um usuário |
| `DELETE` | `/api/usuarios/{id}` | Remove um usuário |

## Modelo

| Campo | Tipo | Regra |
|---|---|---|
| `Id` | `int` | gerado pelo banco |
| `Nome` | `string` | obrigatório, 3–100 caracteres |
| `Email` | `string` | obrigatório, formato válido, único |
| `Idade` | `int` | 0–130 |
| `Cargo` | `string` | obrigatório, até 60 caracteres |
| `Ativo` | `bool` | `true` por padrão |
| `DataCadastro` | `DateTime` | preenchido pelo servidor, em UTC |

## Conceitos praticados

- DTOs de entrada e saída (proteção contra *over-posting*)
- Validação automática via Data Annotations + `[ApiController]`
- Injeção de dependência do `DbContext`
- Filtro dinâmico com `IQueryable`
- Tratamento de conflito (e-mail único → `409 Conflict`)
- Status codes semanticamente corretos (`201` + `Location`, `204`, `404`, `409`)

## Como rodar

```bash
dotnet restore
dotnet run --launch-profile https
```

Depois, acesse `https://localhost:7135/swagger`.

O banco SQLite (`usuarios.db`) é criado automaticamente na primeira execução.
