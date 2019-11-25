FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["./FileServer.csproj", "FileServer/"]
RUN dotnet restore "/src/FileServer/FileServer.csproj"
COPY . "/src/FileServer"
WORKDIR "/src/FileServer"
RUN dotnet build "/src/FileServer/FileServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileServer.dll"]