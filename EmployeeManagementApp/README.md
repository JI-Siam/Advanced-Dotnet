# Employee Management App

A responsive employee management and performance tracking web application built with **ASP.NET Core 10 MVC**, **Entity Framework Core**, and **Bootstrap 5**. Designed to streamline employee record keeping, project evaluations, and automated high-performer classification.

---

## 🚀 Technologies Used

### Backend & Frameworks
- **Framework**: [.NET 10.0](https://dotnet.microsoft.com/) (`net10.0`) ASP.NET Core MVC
- **Language**: C# 13 / .NET 10
- **ORM**: Entity Framework Core 10 (`Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Design`)
- **Database**: Microsoft SQL Server
- **Architecture**: Model-View-Controller (MVC) with Dependency Injection for `EmployeeDbContext`

### Frontend & Styling
- **Template Engine**: ASP.NET Core Razor Views (`.cshtml`)
- **UI Framework**: [Bootstrap 5](https://getbootstrap.com/)
- **Client Libraries**: Bootstrap Bundle & jQuery
- **UI Architecture**: Card-based dashboards, responsive data tables, contextual status badges, and dismissible alert banners

---

## ✨ Key Features

### 1. 👥 Full Employee Lifecycle Management (CRUD)
- **Create Employee (`/Employee/Create`)**:
  - Register new employees with essential data: Employee Name, Employee ID, Performance Score, Lowest Project Rating, and Total Projects Completed.
- **Employee Directory (`/Employee/Index`)**:
  - Interactive table displaying all team members.
  - Performance badges and quick navigation to **Details**, **Edit**, and **Remove** actions.
- **Update Employee (`/Employee/Update/{id}`)**:
  - Edit existing employee credentials, project ratings, and completed projects.
  - Immutable Employee ID to preserve primary key integrity.
  - Error handling with alert notifications if updates encounter issues.
- **Delete Employee (`/Employee/Delete/{id}`)**:
  - One-click record removal with immediate feedback.
- **Employee Details (`/Employee/Details/{id}`)**:
  - Dedicated employee profile card displaying individual performance parameters and automated classification badges.

### 2. 🏆 Automated Performance Classification ("Excellent" vs "Ordinary")
- **Performance Evaluation Engine**:
  - Employees are evaluated against structured corporate criteria:
    - **Performance Score**: $\ge 4.5$
    - **Lowest Project Rating**: $> 3.7$
    - **Total Projects Completed**: $\ge 6$
  - Employees meeting all 3 criteria are classified as **Excellent Employee**, otherwise designated as **Ordinary Employee**.
- **Dedicated "Excellent Employees" Page (`/Employee/Excellent`)**:
  - Filtered showcase of top-tier talent matching the high-performance threshold.
- **Inline High-Performer Highlights**:
  - The main directory includes a filtered sub-view highlighting top scorers ($\text{Performance Score} \ge 4.7$).

### 3. 🔔 Interactive Notifications & Feedback
- Dynamic, dismissible Bootstrap alert banners powered by `TempData`:
  - `CreateMsg`: Confirmation on successful employee creation.
  - `EditMsg`: Status feedback on record updates or error alerts.
  - `deleteMsg`: Notification upon employee removal.

---

## 📊 Database Schema & Model

The application revolves around the `Employee` entity mapped via `EmployeeDbContext`:

| Property | Type | Constraints / Description |
|---|---|---|
| `EmployeeId` | `int` | Primary Key (`EmployeeID`), user-assigned identifier |
| `EmployeeName` | `string` | Maximum length 10 characters |
| `PerformanceScore` | `decimal(3, 1)` | Overall performance score (e.g., 4.8) |
| `LowestProjectRating` | `decimal(3, 1)` | Lowest individual project rating (e.g., 3.9) |
| `TotalProjectsCompleted` | `int` | Count of successfully completed projects |

---

## 📁 Project Structure

```text
EmployeeManagementApp/
├── Controllers/
│   ├── EmployeeController.cs      # Core employee CRUD and performance filter logic
│   └── HomeController.cs          # Landing page & privacy policy
├── EF/
│   ├── EmployeeDbContext.cs       # DbContext configuration & model builder mappings
│   └── Tables/
│       └── Employee.cs            # Employee entity model
├── Models/
│   └── ErrorViewModel.cs
├── Views/
│   ├── Employee/
│   │   ├── Create.cshtml          # Form to add new employee
│   │   ├── Details.cshtml         # Performance breakdown & classification card
│   │   ├── Excellent.cshtml       # Filtered high-performer showcase
│   │   ├── Index.cshtml           # All employees list & inline filters
│   │   └── Update.cshtml          # Form to update employee information
│   ├── Home/
│   │   ├── Index.cshtml           # App landing page
│   │   └── Privacy.cshtml         # Privacy policy
│   └── Shared/
│       ├── _Layout.cshtml         # Global navigation and layout template
│       ├── _ViewImports.cshtml
│       └── _ViewStart.cshtml
├── appsettings.json               # SQL Server connection string
└── Program.cs                     # Service registrations and MVC routing pipeline
```

---

## ⚙️ Getting Started & Setup

### 1. Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server (Local, Azure SQL, or Docker instance)

### 2. Configure Database Connection
Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=EmployeeDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True"
}
```

### 3. Database Scaffolding / Setup (Optional)
If regenerating models from an existing SQL Server database table:
```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=EmployeeDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o EF/Tables --data-annotations --force
```

### 4. Run the Application
Start the development server:
```bash
dotnet run
```
Open your browser and navigate to `https://localhost:5001` (or the HTTP/HTTPS port displayed in your terminal). Click on **Employees** in the navigation bar to begin managing records.
