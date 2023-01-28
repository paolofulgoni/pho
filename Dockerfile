FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Pho.Core/Pho.Core.csproj", "Pho.Core/"]
COPY ["src/Pho.Infrastructure/Pho.Infrastructure.csproj", "Pho.Infrastructure/"]
COPY ["src/Pho.Web/Pho.Web.csproj", "Pho.Web/"]
RUN dotnet restore "Pho.Web/Pho.Web.csproj"
COPY src/. .
WORKDIR "/src/Pho.Web"
RUN dotnet build "Pho.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pho.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pho.Web.dll"]
