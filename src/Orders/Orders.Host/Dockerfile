FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Payments/Payments.Contracts/Payments.Contracts.csproj", "Payments/Payments.Contracts/"]
COPY ["src/Orders/Orders.Contracts/Orders.Contracts.csproj", "Orders/Orders.Contracts/"]
COPY ["src/Orders/Orders.Host/Orders.Host.csproj", "Orders/Orders.Host/"]
RUN dotnet restore "Orders/Orders.Host/Orders.Host.csproj"
COPY ["src/Payments/Payments.Contracts", "Payments/Payments.Contracts/"]
COPY ["src/Orders/Orders.Contracts", "Orders/Orders.Contracts/"]
COPY ["src/Orders/Orders.Host", "Orders/Orders.Host/"]
RUN dotnet publish "Orders/Orders.Host/Orders.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orders.Host.dll"]
