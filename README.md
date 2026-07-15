# HeiwaseWeb

A modern single-page landing site for **Heiwase Karate Szeged**, a local sports club. The site is built with **Blazor WebAssembly** and presents the club’s story, training options, coaches, timetable, and contact information in a mobile-friendly format.

Live site: https://szegedkarate.netlify.app/

## Overview

HeiwaseWeb is a public-facing promotional website for the club. The homepage includes:

- a full-screen hero section with a background video
- a sticky navigation bar for quick scrolling through the page
- sections for training types, about information, coaches, federation details, hall of fame, timetable, and contact
- a simple not-found page for invalid routes

## Built with

- **Blazor WebAssembly**
- **C#**
- **HTML**
- **CSS**
- .NET 10

## Project structure

- `Program.cs` — app bootstrap and service configuration
- `App.razor` — root router and layout wiring
- `Layout/` — shared layout components
- `Pages/Home.razor` — main landing page
- `Pages/NotFound.razor` — fallback page for unknown routes
- `wwwroot/` — static assets such as images, videos, icons, and styles
- `HeiwaseWeb2.csproj` — project configuration

## Features

- Responsive landing page layout
- Smooth, section-based navigation
- Hero video background
- Club information and call-to-action sections
- Public contact-oriented presentation for visitors

## Getting started

### Prerequisites

- .NET 10 SDK

### Run locally

```bash
dotnet restore
dotnet run
```

If your environment uses a different project file name, run the app from the repository root with the solution or project file available in the directory.

### Build for production

```bash
dotnet publish -c Release
```

## Content and customization

Most of the visible content appears to live in `Pages/Home.razor`, while styling and media assets are stored in `wwwroot/`. To update the site:

- edit the page content in `Pages/Home.razor`
- replace images, video, and other static assets in `wwwroot/`
- adjust shared behavior or routing in `App.razor` and `Program.cs`

## Deployment

The project is already deployed at:

https://szegedkarate.netlify.app/

If you deploy it elsewhere, make sure the hosting platform is configured for a client-side Blazor WebAssembly app and serves `index.html` for fallback routes.

## Notes

- The repository currently does not include a license file.
- The project name in the repo is `HeiwaseWeb`, while the main project file is `HeiwaseWeb2.csproj`.

## Contributing

Contributions are welcome. If you make changes, please keep the site fast, mobile-friendly, and focused on the club’s public-facing presentation.

## License

No license has been specified yet.