ASSESSMENT COMPLETION
Notes :-
- The application supports switching to any endpoint for testing (GetPlatformWellActual or GetPlatformWellDummy).
- Swagger UI is integrated for easy testing of the /api/Sync endpoint.

Time taken :-
Approximately 8 hours were spent completing this assessment, in order to:
- Understanding API structure and endpoints
- Building API service and data mapping
- Implementing EF Core Code First with migrations
- Handling dynamic API responses and testing dummy data
- Handling errors while building the code

# Part 1 :-
# DataSync (.NET Core Application) - Complete Setup & Usage

## Project Overview
This .NET 10 Web API application synchronizes data from a REST API into a local SQL Server database (LocalDB).  
It fetches Platform and Well data using a login token, then inserts or updates records in the database while handling missing or optional fields gracefully.  

**Key features:**
- Login to API and obtain Bearer token.
- Fetch Platform & Well data (`GetPlatformWellActual` endpoint).
- Insert or update records in SQL Server LocalDB.
- Handles dynamic API responses (can switch to `GetPlatformWellDummy` endpoint for testing).
- EF Core Code First approach with migrations.

---

## 1️⃣ Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)  
- SQL Server LocalDB (installed with Visual Studio or standalone)  
- Postman or Swagger UI (for API testing)  

---

## 2️⃣ Clone & Open Project
bash
git clone https://github.com/yourusername/DataSync.git
cd DataSync

---
## 3️⃣ Update API Credentials

---

## 4️⃣ Create Database via EF Core

Run these commands to apply migrations and create your LocalDB database:

dotnet ef migrations add InitialCreate
dotnet ef database update

Note: If you add or change models, run a new migration before updating the database.

---
## 5️⃣ Run the Application
dotnet run

By default, Swagger UI will be available at:

http://localhost:5099/swagger/index.html

----
## 6️⃣ Test Sync Endpoint

Open Swagger UI.

Call POST /api/Sync.

Successful response:

Sync completed successfully.

---
## 7️⃣ Switch to Dummy Endpoint (Optional)

To verify handling of missing or extra fields:

Open ApiService.cs.

Change the endpoint from:

"api/PlatformWell/GetPlatformWellActual"


to:

"api/PlatformWell/GetPlatformWellDummy"


Run POST /api/Sync again.

If the application completes without crashing → requirement passed.

----
# Part 2 :-
the SQL Query that would return last updated well for each platform is:-

---sql
SELECT
    p.UniqueName AS PlatformName,
    w.Id,
    w.PlatformId,
    w.UniqueName,
    w.Latitude,
    w.Longitude,
    w.CreatedAt,
    w.UpdatedAt
FROM Wells w
INNER JOIN Platforms p ON w.PlatformId = p.Id
INNER JOIN (
    SELECT PlatformId, MAX(UpdatedAt) AS MaxUpdated
    FROM Wells
    GROUP BY PlatformId
) latest
ON w.PlatformId = latest.PlatformId
AND w.UpdatedAt = latest.MaxUpdated
ORDER BY p.Id;

---

---
