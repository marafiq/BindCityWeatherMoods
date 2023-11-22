FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BindCityWeatherMoods/BindCityWeatherMoods.csproj", "BindCityWeatherMoods/"]
RUN dotnet restore "BindCityWeatherMoods/BindCityWeatherMoods.csproj"
COPY . .
WORKDIR "/src/BindCityWeatherMoods"
RUN dotnet build "BindCityWeatherMoods.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BindCityWeatherMoods.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BindCityWeatherMoods.dll"]
