# Используем базовый образ .NET SDK для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebApplication2.csproj", "./"]
RUN dotnet restore "WebApplication2.csproj"

# Копируем остальные файлы проекта и собираем
COPY . .
WORKDIR "/src/."
RUN dotnet publish "WebApplication2.csproj" -c Release -o /app/publish

# Генерируем итоговый образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication2.dll"]