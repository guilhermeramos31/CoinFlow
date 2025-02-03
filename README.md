# CoinFlow - API

API desenvolvida em **.NET** com **PostgreSQL** e containerizada via **Docker Compose**.

---

## 🚀 Pré-requisitos

Antes de começar, certifique-se de ter os seguintes softwares instalados:

- [Docker](https://docs.docker.com/get-docker/)
- [.NET ](https://dotnet.microsoft.com/pt-br/download) [Documentação](https://learn.microsoft.com/pt-br/dotnet/)

---

## 📥 Como Executar o Projeto

### 1. **Clone o Repositório**
```bash
git clone https://github.com/guilhermeramos31/CoinFlow.git
```

### 2. **Acesse o Diretório do Projeto**
Entre na pasta do projeto recém-clonada:
```bash
cd CoinFlow
```
Isso garante que todos os comandos subsequentes sejam executados dentro do diretório correto.

### 3. **Execute o Docker Compose**
Para iniciar a aplicação com o Docker Compose, execute:
```bash
docker-compose up -d
```
Isso irá construir e subir os containers necessários para a aplicação.

### 4. **Aplicar as migrações ao banco de dados**
```bash
dotnet ef database update
```
### 5. **Agora execute a aplicação**
```bash
dotnet run
```
### 6. **Acesse a API**
```bash
https://localhost:7204
```
Caso queira utilizar pelo swagger
```bash
https://localhost:7204/api-docs/index.html
```


## 📌 Endpoints Disponíveis

### 🔐 Auth

| Método | Endpoint         | Descrição                        |
|--------|-----------------|----------------------------------|
| POST   | `/Auth/Login`   | Autenticação do usuário         |

### 💳 Transaction

| Método | Endpoint                      | Descrição                          |
|--------|--------------------------------|------------------------------------|
| POST   | `/Transaction/Transfer`       | Realizar uma transferência        |
| GET    | `/Transaction/TransferHistory` | Obter histórico de transferências |

### 👤 User

| Método | Endpoint        | Descrição                               |
|--------|----------------|-----------------------------------------|
| POST   | `/User/Register` | Registrar um novo usuário            |
| GET    | `/User/Me`       | Obter informações do usuário autenticado |

### 💰 Wallet

| Método | Endpoint           | Descrição                        |
|--------|-------------------|----------------------------------|
| POST   | `/Wallet/Deposit`  | Realizar um depósito            |
| POST   | `/Wallet/Withdrawal` | Realizar um saque              |
| GET    | `/Wallet/Balance`  | Consultar saldo da carteira     |
