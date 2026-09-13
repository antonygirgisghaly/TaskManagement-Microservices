FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Notifications.Api/Notifications.Api.csproj", "Notifications.Api/"]
COPY ["Notifications.Application/Notifications.Application.csproj", "Notifications.Application/"]
COPY ["Notifications.Domain/Notifications.Domain.csproj", "Notifications.Domain/"]
COPY ["Notifications.Infrastructure/Notifications.Infrastructure.csproj", "Notifications.Infrastructure/"]

RUN dotnet restore "Notifications.Api/Notifications.Api.csproj"

COPY . .

WORKDIR /src/Notifications.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Notifications.Api.dll"]