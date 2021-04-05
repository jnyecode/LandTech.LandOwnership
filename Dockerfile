FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY LandTech.LandOwnership/*.csproj .
RUN dotnet restore 

COPY . ./
RUN dotnet publish -c Release -o published

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app/published .
ENTRYPOINT ["dotnet", "LandTech.LandOwnership.dll"]