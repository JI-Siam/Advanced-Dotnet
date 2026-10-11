# Jasim Plaza Tenant Portal

A modern, role-based commercial property and tenant management web application built with **ASP.NET Core 10 MVC**, **Entity Framework Core**, and **Tailwind CSS**. Designed specifically for commercial plazas to automate monthly rent tracking, streamline shop allocations, and support digital payments via **bKash**.

---

## 🚀 Technologies Used

### Backend & Frameworks
- **Framework**: [.NET 10.0](https://dotnet.microsoft.com/) (`net10.0`) ASP.NET Core MVC
- **Language**: C# 13 / .NET 10
- **ORM**: Entity Framework Core 10 (`Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Design`, `Microsoft.EntityFrameworkCore.Tools`)
- **Database**: Microsoft SQL Server
- **State Management**: ASP.NET Core HTTP Sessions (`ISession`, HttpOnly cookies, 8-hour timeout)
- **HTTP Client**: Typed `HttpClient` (`AddHttpClient<IBkashPaymentService, BkashPaymentService>()`)

### Payment Gateway
- **bKash Tokenized Checkout API**:
  - Token Grant & Refresh (`/token/grant`)
  - Create Payment (`/create`)
  - Execute Payment & Verification (`/execute`)
  - Sandbox/Production configuration toggleable via `appsettings.json`

### Frontend & Styling
- **Template Engine**: ASP.NET Core Razor Pages & MVC Views (`.cshtml`)
- **UI Framework**: Tailwind CSS (loaded via CDN with customized brand color palette)
- **Styling Architecture**: Modern responsive cards, data tables, modal/print layouts, and subtle gradients
- **Icons & Layout**: Fully responsive mobile/desktop navigation bar with toggle menu

---

## ✨ Key Features

### 1. 🔐 Role-Based Access Control (RBAC) & Authentication
- **Admin Role**:
  - Full administrative access to manage shops, tenants, and rent records.
  - Comprehensive KPI dashboard with plaza-wide financial statistics.
  - Access to full invoice identifiers and management controls.
- **Owner (Tenant) Role**:
  - Restricted tenant view: owners only see their assigned shops and related rent records.
  - Owner-safe invoice codes (`INV-{RecordId}-{Month}`) preventing internal database exposure.
  - Direct digital rent payment via bKash.
  - Self-service profile updates.
- **Session-Based Authentication**: Secure cookie-backed session authentication with return URL redirection.

### 2. 📊 Administrative Dashboard
- Real-time metrics overview on home screen:
  - Total registered shops.
  - Total tenant/owners.
  - Count of current-month **Due**, **Pending**, and **Paid** records.
  - Number of distinct shops with pending dues.
- Quick navigation shortcuts to core management modules.

### 3. 🏬 Shop Management (`/Shop`)
- **CRUD Operations**: Admin can register new shops, update location/rent details, and delete shops.
- **Owner Allocation**: Map each shop to a registered property owner/tenant.
- **Data Integrity**: Deleting a shop automatically cascades or cleans up related rent records.
- **Tenant Filter**: Tenants viewing the shop index only see their allocated premises.

### 4. 💳 Rent & Invoice Management (`/Rent`)
- **Automated Monthly Billing**:
  - On application startup, `DbSeeder.EnsureCurrentMonthRentRecords` checks all shops and automatically generates a `Due` rent record for the current month if one doesn't exist.
- **Filtering & Search**:
  - Filter rent records by **Month**, **Year**, **Status** (`Paid`, `Pending`, `Due`), and **Shop Name** (Admin only).
- **Duplicate Prevention**:
  - System prevents creating duplicate rent records for the same shop in the same month/year.
- **Printable Invoices**:
  - Formatted printable invoice page (`/Rent/Invoice`) with Plaza details, tenant info, rent period, breakdown, and payment status stamps.
- **Tamper-Resistant Invoice Codes**:
  - Supports both standard code `INV-{id}-{shopId}-{mm}` and tenant-safe `INV-{id}-{mm}` routing.

### 5. 📱 bKash Payment Gateway Integration (`/Rent/PayWithBkash`)
- Integrates seamlessly with bKash's Tokenized Checkout API.
- Tenants can initiate instant payment directly from their invoice view.
- Automated callback handling (`/Rent/BkashCallback`):
  - Validates payment status and bKash transaction token.
  - Executes payment verification with bKash servers.
  - Automatically marks rent record status as `Paid`.
  - Records the completion date and captures the transaction reference ID.

### 6. 👤 User Management & Profile (`/User`)
- User directory listing categorized by roles (Admin / Owner).
- Self-service profile page to update display name and password.
- Built-in database seeder ensuring default test accounts are available out of the box.

---

## 👥 Demo Credentials

The database automatically seeds test accounts upon startup (`DbSeeder.cs`):

| Role | Email | Password |
|---|---|---|
| **Admin** | `admin@jasimplaza.local` | `admin123` |
| **Owner (Tenant 1)** | `owner@jasimplaza.local` | `owner123` |
| **Owner (Tenant 2)** | `owner2@jasimplaza.local` | `owner123` |

---

## 📁 Project Structure

```text
JasimPlazaTenantPortal/
├── Controllers/
│   ├── HomeController.cs          # Dashboard metrics & landing page
│   ├── RentController.cs          # Rent records, invoices & bKash flows
│   ├── ShopController.cs          # Shop CRUD & tenant assignments
│   └── UserController.cs          # Authentication, profile & user directory
├── Data/
│   └── DbSeeder.cs                # Auto-seeds demo accounts & monthly rent
├── EF/
│   └── Tables/
│       ├── JasimPlazaDbContext.cs # EF Core DbContext
│       ├── Shop.cs                # Shop model
│       ├── RentRecord.cs          # Rent transaction model
│       └── User.cs                # User & authentication model
├── Models/
│   ├── ErrorViewModel.cs
│   ├── LoginViewModel.cs
│   └── UserProfileUpdateModel.cs
├── Services/
│   ├── BkashPaymentService.cs     # bKash API token grant, payment create & execute
│   └── BkashSettings.cs           # bKash configuration bindings
├── Views/
│   ├── Home/                      # Dashboard & information views
│   ├── Rent/                      # Rent list, create, edit, details & invoice
│   ├── Shop/                      # Shop listing & management views
│   ├── User/                      # Login, directory & profile views
│   └── Shared/                    # _Layout, validation scripts, error pages
├── appsettings.json               # Database connections & bKash credentials
└── Program.cs                     # Middleware pipeline & DI registrations
```

---

## ⚙️ Getting Started & Setup

### 1. Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Microsoft SQL Server (Local, Azure SQL, or Docker)

### 2. Configure Database Connection
Update `appsettings.json` with your SQL Server connection details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=JasimPlazaDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True"
}
```

### 3. Database Scaffolding / Initialization
If setting up a fresh database from an existing SQL Server schema:
```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=JasimPlazaDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o EF/Tables --data-annotations --force
```
*(On application run, `db.Database.EnsureCreated()` and `DbSeeder` will automatically populate initial demo users and the current month's rent records).*

### 4. Configure bKash Gateway (Optional)
To test bKash payments, update the `Bkash` section in `appsettings.json`:
```json
"Bkash": {
  "Enabled": true,
  "UseSandbox": true,
  "BaseUrl": "https://tokenized.sandbox.bka.sh/v1.2.0-beta/tokenized/checkout",
  "GrantTokenEndpoint": "/token/grant",
  "CreatePaymentEndpoint": "/create",
  "ExecutePaymentEndpoint": "/execute",
  "Username": "sandbox_username",
  "Password": "sandbox_password",
  "AppKey": "sandbox_app_key",
  "AppSecret": "sandbox_app_secret"
}
```

### 5. Run the Application
Run the web application locally:
```bash
dotnet run
```
Navigate to `https://localhost:5001` or the port shown in your terminal, and log in with any of the demo accounts listed above.
