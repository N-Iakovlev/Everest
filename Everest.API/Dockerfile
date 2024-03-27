
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Everest.API/Everest.API.csproj", "Everest.API/"]
COPY ["Everest.Domain/Everest.Domain.csproj", "Everest.Domain/"]
RUN dotnet restore "Everest.API/Everest.API.csproj"
COPY . .
WORKDIR "/src/Everest.API"
RUN dotnet build "Everest.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Everest.API.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Everest.API/wwwroot", "wwwroot"]
ENTRYPOINT ["dotnet", "Everest.API.dll"]
