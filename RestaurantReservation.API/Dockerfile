# ������������� .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish RestaurantReservation.API/RestaurantReservation.API.csproj -c Release -o out

# ��������� ASP.NET Core �������
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "RestaurantReservation.API.dll"]
