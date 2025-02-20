# 📝 ToDo List  

Este é um projeto de gerenciamento de tarefas desenvolvido com **.NET 9**, seguindo os princípios do **SOLID**, **Clean Code** e **Domain-Driven Design (DDD)**.  

## 🚀 Tecnologias Utilizadas  

- **Back-end**: .NET 9, C#  
- **Banco de Dados**: SQL Server  
- **Arquitetura**: SOLID, Clean Code, DDD  

## 📌 Funcionalidades  

- Criar, editar e excluir tarefas  
- Alterar o status das tarefas (pendente, em andamento, concluída)  
- Listagem de tarefas organizadas  
- Suporte para múltiplos usuários  

## 🔧 Instalação e Execução  

1. **Clone o repositório**  
   ```bash
   git clone https://github.com/nathampa0909/todolist.git
   cd todolist
   ```

2. **Configure a conexão com o banco de dados**  
   - No `appsettings.json`, atualize a **string de conexão** do SQL Server.  

3. **Restaurar pacotes e compilar**  
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Rodar as migrações e atualizar o banco**  
   ```bash
   dotnet ef database update
   ```

5. **Executar o projeto**  
   ```bash
   dotnet run
   ```

## 🛠️ Estrutura do Projeto (DDD)  

- **Domain** → Entidades, agregados e regras de negócio  
- **Application** → Casos de uso e serviços  
- **Infrastructure** → Persistência, repositórios e configurações  
- **API** → Controllers e entrada de dados  

## 🤝 Contribuição  

Contribuições são bem-vindas! Siga as boas práticas e abra uma **issue** ou **pull request**.  

## 📜 Licença  

Este projeto está sob a licença **MIT**.  

