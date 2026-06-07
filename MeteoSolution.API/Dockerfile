# ==========================================
# STAGE 1 — BUILD
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MeteoSolution.API.csproj", "."]
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# ==========================================
# STAGE 2 — RUNTIME
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# Usuário não-root (obrigatório pelo PDF)
RUN useradd -m appuser

WORKDIR /app

COPY --from=build /app/publish .

# Variável de ambiente
ENV ASPNETCORE_ENVIRONMENT=Production

# Porta exposta
EXPOSE 8080

# Usuário não-root
USER appuser

ENTRYPOINT ["dotnet", "MeteoSolution.API.dll"]