# Event Management System 🎟️

A simple web app for managing events and ticket orders. Built with ASP.NET Core MVC in Visual Studio 2022 for a school project.

## Features

**Users can:**
- View all events
- Order tickets for an event
- View their own orders ("My events")

**Admins can:**
- Create events (title, description, date, available tickets)
- View all events and order tickets
- View all orders

**Ordering tickets:**
- Each event has its own order form where you choose the number of tickets
- Clicking **[Order]** creates a new order in the database and lowers the event's available tickets
- You can't order more tickets than are available, and two people ordering at the same time can't oversell an event

The first registered user is made **Admin** the next time the app starts.

## Tech

- ASP.NET Core MVC (.NET 8)
- ASP.NET Core Identity (login, register, roles)
- Entity Framework Core
- SQL Server LocalDB
- Visual Studio 2022

## Project Structure

| Folder / File | Purpose |
|---|---|
| `EventManagementSystem.sln` | Visual Studio solution |
| `EventManagementSystem/Controllers` | `HomeController`, `EventsController`, `AdminController` |
| `EventManagementSystem/Models` | `Event`, `Order`, `ApplicationUser` |
| `EventManagementSystem/Views` | Razor views for events, admin panel and layout |
| `EventManagementSystem/Data` | `ApplicationDbContext` and migrations |
| `EventManagementSystem/Program.cs` | App setup and Admin/User role seeding |
| `EventManagementSystem/appsettings.json` | Database connection string |

## How to Run

1. Clone the repository and open `EventManagementSystem.sln` in Visual Studio 2022.
2. Restore NuGet packages (Visual Studio does this automatically on build).
3. Create the database. The migrations are already included, so in **Package Manager Console** run:
   ```
   Update-Database
   ```
4. Press **F5** to run the project.
5. Register an account, then restart the app — the first account becomes Admin.

Without Visual Studio (needs the .NET 8 SDK and SQL Server LocalDB):

```
dotnet tool install --global dotnet-ef
dotnet ef database update --project EventManagementSystem
dotnet run --project EventManagementSystem
```

## License

This project is licensed under the [MIT License](LICENSE).

## Links

Repository: https://github.com/Danmanbg/Event-Management-System
