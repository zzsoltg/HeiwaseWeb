# HeiwaseWeb

Official serverless website project for a local sports club called **Heiwase Karate Szeged**,
with public website and Content Management System.

## 🌐 Live Deployment

- **Production URL (_public website_):** https://nice-glacier-0b19a2e03.6.azurestaticapps.net
- **Production URL (_private CMS_):** _not yet published_

## ℹ️ Project Purpose

**Goal:** The project aims to provide a unique, modern, open-source internet presence for sports clubs,
with easy-to-use content management for non-technical club leaders and trainers,
while minimizing the cost of hosting and maintenance by using serverless architecture and cloud services.

The project provides a **public-facing club website** with:
- SPA landing page with responsive design and mobile-first layout, following the club's visual identity
- bilingual content support (`hu-HU`, `en-US`)
- CMS edited sections for news, hall of fame
- Application form for new members, with email notifications and admin review

The club leaders, trainers and administrators have private access to the **CMS** for managing content, media, and applications.
The project is designed to be serverless, scalable, and cost-efficient, leveraging Azure services.

## 🛠️ Technology Stack

**Frameworks:**
- .NET 10
- Blazor WebAssembly Standalone
- Azure Functions API
- Class Libraries
- xUnit and bUnit test modules

**Azure Resources:**
- Azure Cosmos NoSQL DB
- Azure Blob Storage
- Azure Static Web Apps

**Languages:**
- C#
- Razor Components with Radzen Blazor UI library and Bootstrap 5
- HTML
- LESS/CSS
- JavaScript
- Python
- Bicep

## 📁 Repository Layout
```text
├── .config/
│   └── dotnet-tools.json             # Dotnet special tools for testing and authentication
├── .github/
│   ├── scripts/                      # Python scripts for GitHub Actions workflows summary
│   └── workflows/                    # GitHub Actions workflows for CI/CD
├── docs/
│   ├── TESTING.md                    # Testing guide locally and in CI/CD
│   └── THESIS_PROPOSAL.md            # Thesis proposal for the project and development plan
├── infra/
│   ├── main.bicep                    # Azure Resource Group configuration for the project
│   └── main.json
├── src/
│   ├── Heiwase.App.Api/              # Azure Functions API project for content, media, and application management
│   ├── Heiwase.App.Blazor/           # Public SPA Blazor WebAssembly Standalone project for the club website
│   ├── Heiwase.App.BlazorAdmin/      # Private CMS Blazor WebAssembly Standalone project for editing content and managing media, with receiving applications and contact messages
│   ├── Heiwase.App.Infrastructure/   # Entities, Repositories, Interfaces and Cosmos DB connection for the API and Blazor projects
│   └── Heiwase.App.Shared/           # Shared DTOs, Enums, and Constants between Business layers (API and Infrastructure) and Presentation layers (Blazor and BlazorAdmin)
├── tests/
│   ├── Heiwase.App.Tests.Blazor/     # bUnit test project for the Blazor SPA and BlazorAdmin CMS, xUnit test for application form logic
│   ├── Heiwase.App.Tests.Unit/       # xUnit test project for unit testing the API and Infrastructure projects
│   ├── .editorconfig                 # Override editorconfig for test projects
│   └── .coverage.runsettings         # Settings for code coverage collection in test projects (excluding Designer files etc.)
├── .editorconfig                     # Override editorconfig for the solution
├── .gitignore                          
├── Directory.Build.props             # Default build properties for the solution
├── Heiwase.App.slnx
├── LICENSE
└── README.md
```

## 📅 Development Progress

| # | Milestone | Deadline | State | Version |
|---|----------|----------|---------|---------|
| 1 | Initialization | 07/31/2026 23:59 | ✅ | `v0.1.0-alpha` |
| 2 | Content, UX & Design System Refresh | 09/30/2026 23:59 | ✅ | `v0.1.0-beta` |
| 3 | Azure Cloud Foundation & Solution Re-Architecture | 10/31/2026 23:59 | ✅ | `v0.2.0-beta` |
| 4 | Public Site: Dynamic Content on Azure | 11/30/2026 23:59 | ⏳ |  |
| 5 | Admin Portal: Secure Content Management System | 12/31/2026 23:59 |  |  |
| 6 | Azure DevOps, Security, Observability & Go-Live | 02/06/2027 23:59 |  |  |

## 🚀 Local Development

### Prerequisites
- .NET 10 SDK
- Azure Functions Core Tools v4

### Local run

**Warning (ongoing development):**
> This repository is currently in development and not yet fully functional.
The backend API is still under development, and the frontend Blazor application is not yet fully implemented.

**Access to the database and blob storage:**
> The following steps are for local development and testing purposes only.
You can attach your own Azure Resource Group to the project via modifying the `infra/main.bicep` file, and change the CMS tenant for the BlazorAdmin project.
Without that, the API will only accept the public GET requests, and the Application Form POST methods and you won't be able to access the CMS Pages.

**1. Clone the repository**
```bash
git clone https://github.com/zzsoltg/HeiwaseWeb
```

**2. Start the backend API**
```bash
cd src/Heiwase.App.Api
func start
```

**3. Start the frontend Blazor application**
```bash
cd ../Heiwase.App.Blazor
dotnet restore
dotnet run
```

### Infrastructure & Local Settings (local development)

To run the API locally, create a `local.settings.json` file inside `src/Heiwase.App.Api/` (this file is git-ignored):

```jsonc
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    
    // Media Storage (Azure Blob / Azurite)
    "Media__AccountName": "heiwasemedia",
    "Media__ContainerName": "public-media",
    "Media__BlobEndpoint": "",

    // Database (Cosmos DB)
    "CosmosDb__KeyVaultUri": "https://<your-keyvault-name>.vault.azure.net/",
    // OR direct connection for local emulator:
    "CosmosDb__ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=..."
  }
}
```

## 🤝 Contribution Policy

This project is currently **not accepting external contributions**.
While you can fork and clone the repository for evaluation, learning, and reference purposes, please do not submit pull requests or issues.

## 📄 License

This repository is licensed under the **HeiwaseWeb Dual-Scope Source-Available License v1.0**.

In short:
- viewing, cloning, and forking for evaluation/learning/reference are allowed
- reuse of architectural ideas is allowed, and code snippets are reusable only from `Heiwase.App.Blazor/**/*.cs` and `Heiwase.App.Blazor/wwwroot/js/**/*.js` (max 40 consecutive lines per snippet, max 120 total lines from one source file)
- direct copying from `.razor`, stylesheet, media, and text resources is prohibited
- copying/reusing the site design, visual identity, media assets, and written page content is prohibited

Users violating these terms will be officially reported to GitHub and face legal charges under EU and Hungarian copyright law.

See `LICENSE` for full terms.
