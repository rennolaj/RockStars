FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /RockStars.API

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o published

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /RockStars.API
COPY --from=build-env /RockStars.API/published .
ENTRYPOINT ["dotnet", "RockStars.API.dll"]
