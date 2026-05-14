FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BrahmanGan.API/BrahmanGan.API.csproj", "src/BrahmanGan.API/"]
COPY ["src/BrahmanGan.Application/BrahmanGan.Application.csproj", "src/BrahmanGan.Application/"]
COPY ["src/BrahmanGan.Domain/BrahmanGan.Domain.csproj", "src/BrahmanGan.Domain/"]
COPY ["src/BrahmanGan.Infrastructure/BrahmanGan.Infrastructure.csproj", "src/BrahmanGan.Infrastructure/"]
RUN dotnet restore "src/BrahmanGan.API/BrahmanGan.API.csproj"
COPY . .
WORKDIR "/src/src/BrahmanGan.API"
RUN dotnet build "BrahmanGan.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BrahmanGan.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrahmanGan.API.dll"]
