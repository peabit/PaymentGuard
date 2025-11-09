FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS publish
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Payments/Payments.Contracts/Payments.Contracts.csproj", "Payments/Payments.Contracts/"] 
COPY ["src/Payments/Payments.Host/Payments.Host.csproj", "Payments/Payments.Host/"] 
RUN dotnet restore "Payments/Payments.Host/Payments.Host.csproj"
COPY ["src/Payments/Payments.Host", "Payments/Payments.Host/"] 
COPY ["src/Payments/Payments.Contracts", "Payments/Payments.Contracts/"] 
RUN dotnet publish "Payments/Payments.Host/Payments.Host.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Payments.Host.dll"]
