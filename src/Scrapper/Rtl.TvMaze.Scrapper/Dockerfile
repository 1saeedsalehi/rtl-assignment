#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

WORKDIR /src


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

COPY . .

WORKDIR "src/Scrapper/Rtl.TvMaze.Scrapper"

RUN dotnet restore "Rtl.TvMaze.Scrapper.csproj"


RUN dotnet build "Rtl.TvMaze.Scrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rtl.TvMaze.Scrapper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rtl.TvMaze.Scrapper.dll"]