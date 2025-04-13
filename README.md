# AuthCrud

Projeto de API REST desenvolvida em .NET 9, com foco em autenticação, autorização, e operações CRUD de usuários. O projeto segue boas práticas de arquitetura, utilizando Repository Pattern e implementa auditoria para ações críticas.

## Tecnologias e Padrões Utilizados

- **.NET 9**
- **Repository Pattern**
- **Autenticação e Autorização com JWT**
- **Auditoria de ações (edição e remoção)**

## Funcionalidades

- Registro de novos usuários
- Autenticação com JWT
- Autorização por token JWT
- CRUD completo de usuários:
  - Criar
  - Listar todos
  - Buscar por ID
  - Editar
  - Remover
- Registro em log/auditoria para edições e exclusões de usuários

## Segurança

- **JWT**: Utilizado para autenticação e autorização nas rotas protegidas.
- **Authorization Middleware**: Protege endpoints contra acesso não autorizado.

## Estrutura do Projeto

O projeto utiliza **Repository Pattern**, separando as responsabilidades de acesso a dados e lógica de negócio.


## Auditoria

Toda ação de **edição** e **remoção** de usuários é registrada para fins de rastreabilidade, incluindo:

- ID do usuário responsável
- Data/hora da ação
- Tipo da operação (edição ou exclusão)
- Dados alterados

## Como rodar o projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/gabriela-ncmt/AuthCrud.git
   ```
2. Acesse o diretório:

```bash
cd AuthCrud
```
3. Configure a connection string no appsettings.json.

4. Execute as migrations (se aplicável) e atualize o banco de dados.

5. Rode o projeto:

```bash

dotnet run

```

## Endpoints principais

| Método | Rota                 | Descrição                   | Autenticação |
|--------|----------------------|-----------------------------|--------------|
| POST   | `/api/auth/register` | Registro de novo usuário    | ❌           |
| POST   | `/api/auth/login`    | Login e geração de token    | ❌           |
| GET    | `/api/users`         | Listar todos os usuários    | ✅           |
| GET    | `/api/users/{id}`    | Buscar usuário por ID       | ✅           |
| PUT    | `/api/users/{id}`    | Editar usuário              | ✅           |
| DELETE | `/api/users/{id}`    | Remover usuário             | ✅           |


