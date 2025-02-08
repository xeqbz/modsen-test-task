FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LibraryApi.Tests/LibraryApi.Tests.csproj", "LibraryApi.Tests/"]
RUN dotnet restore "LibraryApi.Tests/LibraryApi.Tests.csproj"

COPY . .
WORKDIR "/src/LibraryApi.Tests"
RUN dotnet build "LibraryApi.Tests.csproj" -c Release -o /app/build

CMD ["sh", "-c", "sleep 10 && dotnet test --logger:trx"]