

<h1 align="center">
     🏨 <a href="#" alt="site do ecoleta"> Hotel Cancun </a>
</h1>

<h3 align="center">
   A Data-driven API for managing a hotel in Cancun.
</h3>

## 💻 About the project

🔔 HotelCancunAPI - Project developed in one week, with the purpose of applying .Net knowledge in a simple API for reservation management.

---

## ⚙️ Features

- [x] Hotel CRUD
- [x] Suite/Room CRUD
- [x] Register and User Login
- [x] Jwt Authentication 
- [x] Suite booking/reservation
  - [x] Maximum 3 days of booking
  - [x] Maximum 30 days in advance for booking
  - [x] Validation of conflict booking

---
## Prerequisites
Before starting, you will need to have the following tools installed on your machine:

- [.NET 5 SDK](https://docs.microsoft.com/pt-br/dotnet/core/install/windows?tabs=net50), 
- [PostgreSQL 13](https://www.postgresql.org/download/). 
- Besides, it's nice to have an editor to work with code like [Visual Studio](https://visualstudio.com/)

---
## 🎲 Running the API
- Install PostgreSQL Locally
- Create a DB and check the `appsettings.json` on the HotelCancun.Api folder

Example:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Username=postgres;Password=root;Database=db_name;Port=5432;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}

```

- Follow the commands to run the project:
```bash

# Clone the repo
$ git@github.com:rafaxe/hotel-cancun-mvc.git

# Accesse the repo folder
$ cd hotel-cancun-mvc

# Go to API Folder
$ cd HotelCancun.Api

# Go to Data Folder
$ cd ../HotelCancun.Data

# Run the migration to update the db
$ dotnet ef --startup-project ..\HotelCancun.Api\ database update --context AppDbContext       

# Run the Api
$ dotnet watch run 

# The swagger UI will open in your browser
```


---

## 🎨 Swagger

Follow the video to Authenticate user with JWT directly by Swagger (or use a alternative like postman)
<p align="center">
  <img alt="GitHub language count" src="https://github.com/rafaxe/hotel-cancun-mvc/blob/main/swagger-login.gif?raw=true">
</p>

- After creating the admin user, add the manager role permission:
```
INSERT INTO public."AspNetUserRoles"
("UserId", "RoleId")
VALUES({USER_ID}, {ROLE_ID});

// {ROLE_ID} => SELECT "Id" FROM public."AspNetRoles";
```

---

## 🔮 Future implementations
1. Dockerizing postgresql
2. Implementing integration tests - [SpecFlow BDD](https://specflow.org/)
