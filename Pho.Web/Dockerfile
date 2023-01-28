FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Pho.Web/Pho.Web.csproj", "Pho.Web/"]
RUN dotnet restore "Pho.Web/Pho.Web.csproj"
COPY . .
WORKDIR "/src/Pho.Web"
RUN dotnet build "Pho.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pho.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pho.Web.dll"]
