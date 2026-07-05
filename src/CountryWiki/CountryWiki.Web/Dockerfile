FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5082

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/CountryContracts/", "CountryContracts/"]
COPY ["src/CountryShared/CountryShared.Grpc/CountryShared.Grpc.csproj", "CountryShared/CountryShared.Grpc/"]
COPY ["src/CountryWiki/CountryWiki.Domain/CountryWiki.Domain.csproj", "CountryWiki/CountryWiki.Domain/"]
COPY ["src/CountryWiki/CountryWiki.DAL/CountryWiki.DAL.csproj", "CountryWiki/CountryWiki.DAL/"]
COPY ["src/CountryWiki/CountryWiki.BLL/CountryWiki.BLL.csproj", "CountryWiki/CountryWiki.BLL/"]
COPY ["src/CountryWiki/CountryWiki.Web/CountryWiki.Web.csproj", "CountryWiki/CountryWiki.Web/"]

RUN dotnet restore "CountryWiki/CountryWiki.Web/CountryWiki.Web.csproj"

COPY ["src/CountryShared/", "CountryShared/"]
COPY ["src/CountryWiki/", "CountryWiki/"]

WORKDIR "/src/CountryWiki/CountryWiki.Web"

RUN dotnet publish "CountryWiki.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CountryWiki.Web.dll"]