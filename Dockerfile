FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MoscowArts/MoscowArts.csproj", "MoscowArts/"]
RUN dotnet restore "MoscowArts/MoscowArts.csproj"
COPY . .
WORKDIR "/src/MoscowArts"
RUN dotnet build "MoscowArts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoscowArts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoscowArts.dll"]
