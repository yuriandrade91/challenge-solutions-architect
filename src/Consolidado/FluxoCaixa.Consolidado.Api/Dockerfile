FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Consolidado/FluxoCaixa.Consolidado.Api/FluxoCaixa.Consolidado.Api.csproj", "Consolidado/FluxoCaixa.Consolidado.Api/"]
COPY ["src/Consolidado/FluxoCaixa.Consolidado.Application/FluxoCaixa.Consolidado.Application.csproj", "Consolidado/FluxoCaixa.Consolidado.Application/"]
COPY ["src/Consolidado/FluxoCaixa.Consolidado.Domain/FluxoCaixa.Consolidado.Domain.csproj", "Consolidado/FluxoCaixa.Consolidado.Domain/"]
COPY ["src/Consolidado/FluxoCaixa.Consolidado.Infrastructure/FluxoCaixa.Consolidado.Infrastructure.csproj", "Consolidado/FluxoCaixa.Consolidado.Infrastructure/"]
COPY ["src/Lancamentos/FluxoCaixa.Lancamentos.Domain/FluxoCaixa.Lancamentos.Domain.csproj", "Lancamentos/FluxoCaixa.Lancamentos.Domain/"]
COPY ["src/Shared/FluxoCaixa.Shared/FluxoCaixa.Shared.csproj", "Shared/FluxoCaixa.Shared/"]
RUN dotnet restore "Consolidado/FluxoCaixa.Consolidado.Api/FluxoCaixa.Consolidado.Api.csproj"
COPY src/ .
RUN dotnet build "Consolidado/FluxoCaixa.Consolidado.Api/FluxoCaixa.Consolidado.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consolidado/FluxoCaixa.Consolidado.Api/FluxoCaixa.Consolidado.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoCaixa.Consolidado.Api.dll"]
