# Базовий образ для .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Копіюємо .sln та .csproj файли і відновлюємо залежності
COPY MoodCat.sln ./
COPY TelegramBot/TelegramBot.csproj ./TelegramBot/
RUN dotnet restore

# Копіюємо все і будуємо проєкт
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

# Вказуємо команду запуску
ENTRYPOINT ["dotnet", "TelegramBot.dll"]
