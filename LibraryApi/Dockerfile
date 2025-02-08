FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LibraryApi/LibraryApi.csproj", "LibraryApi/"]
COPY ["LibraryApi.Application/LibraryApi.Application.csproj", "LibraryApi.Application/"]
COPY ["LibraryApi.Domain/LibraryApi.Domain.csproj", "LibraryApi.Domain/"]
COPY ["LibraryApi.Infrastructure/LibraryApi.Infrastructure.csproj", "LibraryApi.Infrastructure/"]
RUN dotnet restore "LibraryApi/LibraryApi.csproj"

COPY . .
WORKDIR "/src/LibraryApi"
RUN dotnet build "LibraryApi.csproj" -c Release -o /app/build

RUN dotnet publish "LibraryApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LibraryApi.dll"]