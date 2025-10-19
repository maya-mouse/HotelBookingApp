# Hotel Booking App

---

## Default Users (Seed Data)

When the database is initialized (`DbInitializer`), two default users are created:

| Role   | Email             | Password   |
|--------|-------------------|-------------|
| Admin  | admin@test.com    | Admin123!   |
| Client | client@test.com   | Client123!  |

---

## Architecture 

**Clean Architecture Layers:**
- **Domain** – Core business logic and entities  
- **Application** – Business rules and use cases  
- **Infrastructure** – Data access, EF Core, Dapper, MySQL  
- **Presentation** – UI layer (Razor Pages + Tailwind CSS)

**Tech Stack**
- **Database:** MySQL  
- **ORM / Data Access:** Entity Framework Core + Dapper  
- **Frontend:** Razor Pages + Tailwind CSS  
- **Backend:** ASP.NET Core (.NET 8 )

---
## Overview
<img width="1613" height="826" alt="image" src="https://github.com/user-attachments/assets/32ca900e-076b-4dc4-933e-fded7b104136" />
<img width="1556" height="676" alt="image" src="https://github.com/user-attachments/assets/4fb98311-bc10-4139-8f54-27df339bba65" />
<img width="1560" height="609" alt="image" src="https://github.com/user-attachments/assets/b46a5b07-0218-4336-8779-5ff41c484b54" />
<img width="1541" height="712" alt="image" src="https://github.com/user-attachments/assets/72d1216b-ac46-4970-ae21-42c166e224cb" />




##  Launch Guide

###  Run Tailwind (CSS watcher)
```bash
cd Presentation
npm install
npm run watch:css

{
  "ConnectionStrings": {
    "Default": "Server=localhost;Port=3306;Database=hotel_booking;User=appuser;Password=app_password;"
  }
}
Replace appuser/app_password with your local MySQL credentials

cd Infrastructure 
dotnet ef database update --startup-project ..\Presentation\Presentation.csproj

cd ..\Presentation
dotnet run

