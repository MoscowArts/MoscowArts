#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8000
EXPOSE 4433

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MoscowArts/MoscowArts.csproj", "MoscowArts/"]
RUN dotnet restore "MoscowArts/MoscowArts.csproj"
COPY . .
WORKDIR "/src/MoscowArts"
RUN dotnet build "MoscowArts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoscowArts.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoscowArts.dll"]