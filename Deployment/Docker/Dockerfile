# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY PersonalExpenses/*.csproj ./PersonalExpenses/
RUN dotnet restore ./PersonalExpenses/PersonalExpenses.csproj

# Copy all files and build
COPY PersonalExpenses/. ./PersonalExpenses/
WORKDIR /src/PersonalExpenses
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PersonalExpenses.dll"]
