#(Build Stage)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["Identity.Api/Identity.Api.csproj", "Identity.Api/"]
COPY ["Identity.Application/Identity.Application.csproj", "Identity.Application/"]
COPY ["Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity.Infrastructure/"]

RUN dotnet restore "Identity.Api/Identity.Api.csproj"


COPY . .

WORKDIR /src/Identity.Api
RUN dotnet publish -c Release -o /app/publish

#(Runtime Stage)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Identity.Api.dll"]