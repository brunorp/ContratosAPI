<h2 align="center">
  ContratosAPI
</h2>

<p align="center">Basic web API designed to train some knowledge in ASP.NET Core.</p>

<p align="center">
  <a href="https://github.com/brunorp">
    <img alt="Made by Bruno Rossetto" src="https://img.shields.io/badge/made%20by-Bruno%20Rossetto-blue">
  </a>

  <img alt="Last Commit" src="https://img.shields.io/github/last-commit/brunorp/ContratosAPI">

  <img alt="Contributors" src="https://img.shields.io/github/contributors/brunorp/ContratosAPI">
</p>


## Table of Contents

<ul>
  <li><a href="#-getting-started">Getting Started</a></li>
  <li><a href="#-features">Features</a></li>
  <li><a href="#-support">Support</a></li>
</ul>

---

## 🚀 Getting Started

### Prerequisites

- [.NET Core 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

### Setup
-   Clone/Download the repository.
-   Go to the repository folder and run:
```bash
    dotnet run
``` 

## 📋 Features

- [x] Entity Contrato, with: id (auto-increment), data contratação, quantidade de parcelas, valor financiado, prestações.
- [x] entity Prestação, with: contrato, data vencimento, data pagamento, valor, status (Aberta, Baixada, Atrasada).
- [x] the Status field should be displayed based on the field data vencimento, data atual and data pagamento, not being stored on the database.
- [x] InMemoryDB.
- [x] RESTful API (create, read, update, delete).
- [ ] SOLID, clean code..
- [ ] Unit Testing.
- [x] Contract ID field must not be available to be registered in the post method.
- [x] InMemoryCache.
- [x] Swagger.
- [x] Feature Flags.

### Build with

- Core
  - [.NET Core 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
- Swagger
  - [Swagger](https://swagger.io/) 

---

## 📌 Support

- Linkedin at [Bruno Rossetto](https://www.linkedin.com/in/bruno-rossetto/)
