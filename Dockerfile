##
## STAGE: frontend
##
FROM node:alpine as frontend
WORKDIR /Source

# npm clean install (installs exact versions from package-lock.json)
COPY Frontend/package.json ./
COPY Frontend/package-lock.json ./
RUN npm ci

COPY Frontend/. ./
RUN npm run build:Debug -- --output-path /Build
ENTRYPOINT npm run build:Watch -- --output-path /Build


##
## STAGE: build
##
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY Disunity.Store/*.csproj ./asp/
RUN dotnet restore asp

# copy frontend
COPY --from=frontend /Build/. ./asp/wwwroot

# copy everything else and build app
COPY Disunity.Store/. ./asp/
WORKDIR /app/asp
RUN dotnet publish -c Release -o out


##
## STAGE: runtime
##
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/asp/out ./
ENTRYPOINT ["dotnet", "Disunity.Store.dll"]
