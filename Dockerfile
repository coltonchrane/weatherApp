# Build Stage for Angular Frontend
FROM node:20-alpine AS build-frontend
WORKDIR /app/client
COPY angularwithasp.client/package*.json ./
RUN npm install
COPY angularwithasp.client/ ./
RUN npm run build -- --configuration production

# Build Stage for .NET Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend
WORKDIR /app/server
COPY AngularWithASP.Server/*.csproj ./
RUN dotnet restore
COPY AngularWithASP.Server/ ./
RUN dotnet publish -c Release -o out

# Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-backend /app/server/out ./
COPY --from=build-frontend /app/angularwithasp.client/dist/weatherApp.client/browser ./wwwroot
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "weatherApp.Server.dll"]
