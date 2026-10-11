# .NET & ASP.NET Core Practice & Learning Repository

Welcome to the **Advanced .NET Practice & Learning Repository**! 🚀

This repository serves as a hands-on learning lab and collection of practice projects exploring modern **.NET 10**, **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server** database integration, third-party APIs (including payment gateways), and web development architectures.

---

## 📚 Repository Overview

The projects in this repository track practical progress from core MVC foundations and database connectivity to production-grade role-based web applications:

| Project | Type | Key Topics & Focus | Documentation |
|---|---|---|---|
| **[JasimPlazaTenantPortal](./JasimPlazaTenantPortal)** | Web App | Full-stack commercial plaza management, RBAC (Admin & Tenants), automated monthly billing, bKash payment gateway, Tailwind CSS | [Read Details](./JasimPlazaTenantPortal/README.md) |
| **[EmployeeManagementApp](./EmployeeManagementApp)** | Web App | Employee lifecycle (CRUD), automated performance tier classification, LINQ queries, Bootstrap 5 UI | [Read Details](./EmployeeManagementApp/README.md) |
| **[DatabaseConnection](./DatabaseConnection)** | Practice Module | Connecting ASP.NET Core MVC to SQL Server, DTOs, connection string management, data retrieval | — |
| **[FormValidation](./FormValidation)** | Practice Module | ASP.NET Core Model Binding, Data Annotations, client & server-side validation rules | — |
| **[PractiseProject](./PractiseProject)** | Practice Module | Entity Framework Core experimentation, DbContext setup, and repository / CRUD logic | — |
| **[MyMvcApp](./MyMvcApp) / [MyMvcApp1](./MyMvcApp1)** | Starter Labs | Fundamental MVC structure, Razor views, routing, layout pages, and CLI scaffolding (`dotnet new mvc`) | — |
| **[WebApplication1](./WebApplication1)** | Starter Lab | Initial ASP.NET Core web application setup and experiments | — |

---

## 🌟 Featured Applications

### 1. 🏢 [Jasim Plaza Tenant Portal](./JasimPlazaTenantPortal)
A complete commercial property and tenant portal for managing shopping plazas:
- **Role-Based Access Control (RBAC)**: Distinct permissions for `Admin` (plaza management) and `Owner` (tenants).
- **Automated Monthly Billing**: Background seeding logic that automatically issues rent invoices on the 1st of each month.
- **bKash Tokenized Checkout Integration**: End-to-end digital payment flow with OAuth token grant, payment creation, execution, and automatic transaction recording.
- **Tamper-Resistant Invoices**: Masked invoice codes to avoid database ID exposure and printable invoice layouts.
- **Modern UI**: Styled with responsive Tailwind CSS.

👉 *See full details in the [JasimPlazaTenantPortal README](./JasimPlazaTenantPortal/README.md).*

---

### 2. 👥 [Employee Management App](./EmployeeManagementApp)
An employee operations and performance analytics application:
- **Full Employee CRUD**: Create, edit, inspect, and delete employee records with immediate `TempData` alert feedback.
- **Automated Performance Tier Engine**: Evaluates employees against corporate benchmarks (Performance Score $\ge 4.5$, Lowest Project Rating $> 3.7$, Projects $\ge 6$) to classify them as **Excellent** or **Ordinary**.
- **Performance Filtering**: Dedicated "Excellent Employees" showcase and filtered directory views.
- **Clean UI**: Built with Bootstrap 5 cards, badges, and modal layouts.

👉 *See full details in the [EmployeeManagementApp README](./EmployeeManagementApp/README.md).*

---

## 💡 Key Skills & Concepts Practiced

- **ASP.NET Core 10 MVC Architecture**:
  - Model-View-Controller separation of concerns.
  - Razor view engine (`.cshtml`), tag helpers, partial views, and shared layout templates.
  - Dependency Injection (DI) for database contexts, configuration options, and typed HTTP clients.
- **Entity Framework Core (EF Core 10)**:
  - Code-first modeling and database scaffolding (`dotnet ef dbcontext scaffold`).
  - LINQ queries (`Where`, `Include`, `ThenInclude`, projections, groupings).
  - Relationship management (one-to-many, cascading behaviors).
- **Authentication & Security**:
  - Session-based state management with secure `HttpOnly` cookies.
  - Role-based authorization filters and tenant-level access restrictions.
  - Anti-forgery token verification (`[ValidateAntiForgeryToken]`).
- **External API & Payment Integration**:
  - Typed `HttpClient` service pattern.
  - bKash Tokenized Checkout API lifecycle (Grant Token $\rightarrow$ Create Payment $\rightarrow$ Callback $\rightarrow$ Execute Payment).
- **Data Validation & Error Handling**:
  - Data Annotations for server-side validation and Model State checking (`ModelState.IsValid`).
  - User feedback via `TempData` across redirects.

---

## 🛠️ Tech Stack Across Projects

- **Runtime & Language**: .NET 10.0 (`net10.0`), C# 13
- **Web Framework**: ASP.NET Core MVC
- **Data Access**: Entity Framework Core 10 (`Microsoft.EntityFrameworkCore.SqlServer`)
- **Database**: Microsoft SQL Server
- **Styling & Frontend**: Tailwind CSS, Bootstrap 5, HTML5/CSS3, JavaScript / jQuery

---

## 🚀 Getting Started

### Prerequisites
1. Install [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
2. Install [Microsoft SQL Server](https://www.microsoft.com/sql-server) (or run via Docker)

### Running Any Project
1. Clone or navigate to the repository:
   ```bash
   cd /Applications/dotnet
   ```
2. Navigate into the desired project folder:
   ```bash
   cd JasimPlazaTenantPortal
   # or
   cd EmployeeManagementApp
   ```
3. Verify or update the SQL Server connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost,1433;Database=YourDb;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True"
   }
   ```
4. Restore dependencies and run the project:
   ```bash
   dotnet restore
   dotnet run
   ```
5. Open the local URL displayed in the terminal (typically `https://localhost:5001` or `http://localhost:5000`).

---

## 📁 Repository Structure

```text
dotnet/
├── JasimPlazaTenantPortal/   # Full-featured tenant & rent management portal (bKash, Tailwind)
├── EmployeeManagementApp/     # Employee management & performance rating app (Bootstrap 5)
├── DatabaseConnection/        # Database connectivity & DTO practice
├── FormValidation/            # Data Annotations & model validation practice
├── PractiseProject/           # EF Core context and CRUD experimentation
├── MyMvcApp/                  # Starter MVC learning lab
├── MyMvcApp1/                 # Starter MVC learning lab
├── WebApplication1/           # ASP.NET Core project setup lab
├── dotnet.sln                 # Visual Studio Solution file
└── readme.md                  # Main repository documentation
```

---

*Note: This repository is continuously updated as new .NET features, design patterns, and practice scenarios are explored.*
