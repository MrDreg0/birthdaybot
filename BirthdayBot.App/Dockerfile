FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY BirthdayBot.App/BirthdayBot.App.csproj BirthdayBot.App/
COPY BirthdayBot/BirthdayBot.csproj BirthdayBot/
COPY BirthdayBot.Database/BirthdayBot.Database.csproj BirthdayBot.Database/

RUN dotnet restore BirthdayBot.App/BirthdayBot.App.csproj

COPY BirthdayBot.App BirthdayBot.App
COPY BirthdayBot BirthdayBot
COPY BirthdayBot.Database BirthdayBot.Database

RUN dotnet build BirthdayBot.App/BirthdayBot.App.csproj -c $BUILD_CONFIGURATION --no-restore --no-self-contained

FROM build AS publish
RUN dotnet publish BirthdayBot.App/BirthdayBot.App.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BirthdayBot.App.dll"]
