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

# Install Mono
RUN apt update && apt install -y apt-transport-https dirmngr gnupg ca-certificates && \
        apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
        echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list && \
        apt update && apt install -y mono-devel

WORKDIR /app

# copy csproj and restore as distinct layers
COPY .paket/ ./.paket/
COPY paket.dependencies ./
COPY paket.lock ./
COPY Disunity.Store/*.csproj ./asp/
COPY Disunity.Store/paket.references ./asp/
RUN mono .paket/paket.exe install

# copy frontend
COPY --from=frontend /Build/. ../Frontend/dist

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
