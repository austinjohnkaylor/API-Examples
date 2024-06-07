FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["School System using ASPNetCore Identity/IdentityAPI/IdentityAPI.csproj", "School System using ASPNetCore Identity/IdentityAPI/"]
RUN dotnet restore "School System using ASPNetCore Identity/IdentityAPI/IdentityAPI.csproj"
COPY . .
WORKDIR "/src/School System using ASPNetCore Identity/IdentityAPI"
RUN dotnet build "IdentityAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityAPI.dll"]
