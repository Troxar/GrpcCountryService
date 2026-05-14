FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5076
EXPOSE 5080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/CountryContracts/", "CountryContracts/"]
COPY ["src/CountryService/CountryService.Domain/CountryService.Domain.csproj", "CountryService/CountryService.Domain/"]
COPY ["src/CountryService/CountryService.DAL/CountryService.DAL.csproj", "CountryService/CountryService.DAL/"]
COPY ["src/CountryService/CountryService.BLL/CountryService.BLL.csproj", "CountryService/CountryService.BLL/"]
COPY ["src/CountryService/CountryService.Grpc/CountryService.Grpc.csproj", "CountryService/CountryService.Grpc/"]

RUN dotnet restore "CountryService/CountryService.Grpc/CountryService.Grpc.csproj"

COPY ["src/CountryService/", "CountryService/"]

WORKDIR "/src/CountryService/CountryService.Grpc"

RUN dotnet publish "CountryService.Grpc.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CountryService.Grpc.dll"]