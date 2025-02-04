# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["LibraryApi/LibraryApi.csproj", "LibraryApi/"]
RUN dotnet restore "LibraryApi/LibraryApi.csproj"

# Копируем все файлы и собираем проект
COPY . .
WORKDIR "/src/LibraryApi"
RUN dotnet build "LibraryApi.csproj" -c Release -o /app/build

# Публикуем проект
RUN dotnet publish "LibraryApi.csproj" -c Release -o /app/publish

# Используем официальный образ .NET Runtime для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LibraryApi.dll"]