FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Tasks.Api/Tasks.Api.csproj", "Tasks.Api/"]
COPY ["Tasks.Application/Tasks.Application.csproj", "Tasks.Application/"]
COPY ["Tasks.Domain/Tasks.Domain.csproj", "Tasks.Domain/"]
COPY ["Tasks.Infrastructure/Tasks.Infrastructure.csproj", "Tasks.Infrastructure/"]

RUN dotnet restore "Tasks.Api/Tasks.Api.csproj"

COPY . .

WORKDIR /src/Tasks.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Tasks.Api.dll"]