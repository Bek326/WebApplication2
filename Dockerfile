# Используем базовый образ .NET SDK для сборки проекта
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
# Указываем путь относительно контекста сборки Docker, который должен быть установлен на уровне директории WebApplication2/
COPY ["WebApplication2/WebApplication2.csproj", "WebApplication2/"]
RUN dotnet restore "WebApplication2/WebApplication2.csproj"

# Копируем остальные файлы проекта и собираем
COPY ["WebApplication2/", "WebApplication2/"]
RUN dotnet publish "WebApplication2/WebApplication2.csproj" -c Release -o /app/publish

# Генерируем итоговый образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApplication2.dll"]