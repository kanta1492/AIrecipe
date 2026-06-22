FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AiMealPlanner.csproj", "./"]
RUN dotnet restore "./AiMealPlanner.csproj"

COPY . .
RUN dotnet publish "./AiMealPlanner.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

RUN mkdir -p /var/data/keys /var/data/uploads

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AiMealPlanner.dll"]
