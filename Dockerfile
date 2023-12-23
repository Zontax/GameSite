FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 59518
EXPOSE 44364

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY GameSite/GameSite.csproj GameSite/
RUN dotnet restore GameSite/GameSite.csproj
COPY . .
WORKDIR /src/GameSite
RUN dotnet build GameSite.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GameSite.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GameSite.dll"]