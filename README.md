# HeiwaseWeb

Official website project for **Heiwase Karate Szeged**, implemented as a Blazor WebAssembly single-page application.

## Live Deployment

**Production URL:** https://nice-glacier-0b19a2e03.6.azurestaticapps.net

> Legacy Netlify hosting is being phased out in favor of Azure Static Web Apps.

## Project Purpose

The project provides a public-facing club website with:
- hero/landing presentation
- training and program overview
- coach and club background sections
- hall of fame content
- timetable and contact sections
- bilingual content support (`hu-HU`, `en-US`)

## Current Codebase Condition (Measured)

Based on the current repository state:

- **Architecture:** single-project .NET solution with one Blazor WebAssembly app (`Heiwase.App.Blazor`).
- **Frontend composition:** modular Razor component structure under `Components/Pages`, `Components/Layout`, and `Components/Shared`.
- **Localization:** culture selection with resource-based localization and language-specific Razor component variants.
- **UI/Assets:** static site UI with extensive media usage; media has already been moved to Azure Blob Storage URLs in component markup.
- **Deployment automation:** Azure Static Web Apps workflow exists at `.github/workflows/azure-static-web-apps-nice-glacier-0b19a2e03.yml`.
- **Platform status:** frontend is functional and deployable; larger multi-project/API/admin architecture is planned but not yet present in this branch.

## Backlog Analysis

### Completed Work (from closed backlog items)
Recent and historical completed items indicate that the team has already delivered:

- core landing-page feature set and componentized structure
- localization support
- modal/dialog-based UX improvements
- contact/form-related improvements
- responsive and styling bug fixes
- favicon and multiple presentation/content fixes
- media migration/support work toward Azure hosting readiness

### Pending Work (from open backlog items)
The open backlog shows major roadmap focus areas:

1. **Solution expansion**
   - move from single project to multi-project structure
   - scaffold shared, API, infrastructure, and admin applications
2. **Cloud data/content platform**
   - provision Azure resources
   - design Cosmos DB containers and repository layer
   - implement Blob upload services and seed existing data
3. **Identity & security**
   - register Entra ID app
   - add admin authentication and API authorization
   - establish secrets/security baseline
4. **Public/API integration**
   - expose articles, hall-of-fame, and application endpoints
   - connect public site sections to Azure-backed APIs
5. **Operations**
   - CI/CD hardening (dual workflows, OIDC migration)
   - observability (Application Insights) and cost governance
   - domain/TLS cutover and decommissioning Netlify/Formspree
6. **Content enhancements**
   - dojo section, media replacement, copy rewrites, gallery/animation refinements

## Technology Stack

- .NET 10
- Blazor WebAssembly
- C#
- Razor Components
- LESS/CSS
- JavaScript
- Radzen Blazor Components

## Repository Layout

- `Heiwase.App.Blazor/Program.cs` – app bootstrap, services, localization culture setup
- `Heiwase.App.Blazor/App.razor` – routing/root composition
- `Heiwase.App.Blazor/Components/` – page, layout, and shared UI components
- `Heiwase.App.Blazor/Resources/` – localization resources
- `.github/workflows/` – CI/CD workflow definition

## Local Development

### Prerequisites
- .NET 10 SDK

### Run

```bash
dotnet restore
dotnet run --project Heiwase.App.Blazor/Heiwase.App.Blazor.csproj
```

### Publish

```bash
dotnet publish Heiwase.App.Blazor/Heiwase.App.Blazor.csproj -c Release
```

## Contribution Policy

This project is currently **not accepting external contributions**.

## License

This repository is licensed under the **HeiwaseWeb Content & Design Protection License v1.0**.

In short:
- viewing, cloning, and forking for evaluation/learning/reference are allowed
- reuse of architectural ideas is allowed, and code snippets are reusable only from `Heiwase.App.Blazor/**/*.cs` and `Heiwase.App.Blazor/wwwroot/js/**/*.js` (max 40 consecutive lines per snippet, max 120 total lines from one source file)
- direct copying from `.razor`, stylesheet, media, and text resources is prohibited
- copying/reusing the site design, visual identity, media assets, and written page content is prohibited

See `LICENSE` for full terms.
