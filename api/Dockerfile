FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore
# Copy everything else and build the project
COPY . ./
RUN dotnet publish -c Release -o out
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
# Set environment variables
EXPOSE 80
ENTRYPOINT ["dotnet", "api.dll"]