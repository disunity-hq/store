FROM node:alpine as frontend
WORKDIR /app

COPY Disunity.Store/. ./
RUN ls -la
RUN npm install
RUN npm run build:Debug
RUN ls -la

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

RUN apt-get update -y && apt-get -y install curl software-properties-common
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get install -y nodejs

# copy csproj and restore as distinct layers
COPY Disunity.Store/*.csproj ./asp/
COPY Disunity.Store/package.json ./asp/
RUN dotnet restore asp

# copy frontend
COPY --from=frontend /app/wwwroot/dist/. ./asp/wwwroot/dist/
RUN ls -la asp/wwwroot/

# copy everything else and build app
COPY Disunity.Store/. ./asp/
WORKDIR /app/asp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/asp/out ./
ENTRYPOINT ["dotnet", "Disunity.Store.dll"]
