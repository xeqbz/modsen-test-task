# Используем официальный образ .NET SDK для сборки и запуска тестов
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY ["LibraryApi.Tests/LibraryApi.Tests.csproj", "LibraryApi.Tests/"]
RUN dotnet restore "LibraryApi.Tests/LibraryApi.Tests.csproj"

# Копируем все файлы и собираем проект
COPY . .
WORKDIR "/src/LibraryApi.Tests"
RUN dotnet build "LibraryApi.Tests.csproj" -c Release -o /app/build

# Запускаем тесты
ENTRYPOINT ["dotnet", "test", "--logger:trx"]