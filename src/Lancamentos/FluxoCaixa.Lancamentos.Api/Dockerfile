FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Lancamentos/FluxoCaixa.Lancamentos.Api/FluxoCaixa.Lancamentos.Api.csproj", "Lancamentos/FluxoCaixa.Lancamentos.Api/"]
COPY ["src/Lancamentos/FluxoCaixa.Lancamentos.Application/FluxoCaixa.Lancamentos.Application.csproj", "Lancamentos/FluxoCaixa.Lancamentos.Application/"]
COPY ["src/Lancamentos/FluxoCaixa.Lancamentos.Domain/FluxoCaixa.Lancamentos.Domain.csproj", "Lancamentos/FluxoCaixa.Lancamentos.Domain/"]
COPY ["src/Lancamentos/FluxoCaixa.Lancamentos.Infrastructure/FluxoCaixa.Lancamentos.Infrastructure.csproj", "Lancamentos/FluxoCaixa.Lancamentos.Infrastructure/"]
COPY ["src/Shared/FluxoCaixa.Shared/FluxoCaixa.Shared.csproj", "Shared/FluxoCaixa.Shared/"]
RUN dotnet restore "Lancamentos/FluxoCaixa.Lancamentos.Api/FluxoCaixa.Lancamentos.Api.csproj"
COPY src/ .
RUN dotnet build "Lancamentos/FluxoCaixa.Lancamentos.Api/FluxoCaixa.Lancamentos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lancamentos/FluxoCaixa.Lancamentos.Api/FluxoCaixa.Lancamentos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoCaixa.Lancamentos.Api.dll"]
