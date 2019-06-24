##
## STAGE: frontend
##
FROM node:alpine as frontend
WORKDIR /Source

COPY Disunity.Store/StaticFiles/package.json ./
RUN npm install

COPY Disunity.Store/StaticFiles/. ./
RUN npm run build:Debug
ENTRYPOINT npm run build:Watch

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
