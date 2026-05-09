# WeatherApp Project Instructions

WeatherApp is a full-stack sandbox project providing weekly weather forecasts based on user location.

## Project Overview
This application retrieves the user's geolocation via the browser and fetches weather forecasts from external APIs (Open-Meteo, Geocode.maps.co). It serves as a demonstration for a modern full-stack architecture and sophisticated CI/CD pipelines.

## Architecture & Tech Stack
- **Backend**: ASP.NET Core 10.0 (Web API).
- **Frontend**: Angular 19.
- **Languages**: C#, TypeScript.
- **External APIs**: Open-Meteo (Weather), Geocode.maps.co (Geocoding).
- **Infrastructure**: Docker (Multi-stage builds).
- **CI/CD**: GitHub Actions (Deployments to Azure Web Apps and local Proxmox-hosted IIS via Tailscale). Azure deployments use standard public internet access, while Tailscale provides secure access to the local Proxmox environment.

### Core Architecture
- **`AngularWithASP.Server/`**: The ASP.NET Core backend. It acts as a proxy for weather data and serves the Angular static files in production.
- **`angularwithasp.client/`**: The Angular frontend. It handles user interaction, geolocation, and displays the weather forecast.
- **SPA + Web API Pattern**: The backend provides the data API while also hosting the frontend client.

## Development Workflows
- **Running Locally**:
    - **Visual Studio**: Open `weatherApp.sln` and run the project.
    - **CLI**:
        - Backend: `dotnet run --project AngularWithASP.Server/AngularWithASP.Server.csproj`
        - Frontend: `cd angularwithasp.client && npm install && npm start`
- **Docker**: Build and run the entire stack using the root `Dockerfile`.
- **Deployment**: Automatic deployment to Azure and Proxmox occurs on pushes to the `main` branch.
