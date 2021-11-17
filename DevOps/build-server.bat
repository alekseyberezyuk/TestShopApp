cd ..
cd TestShopApp-Api\TestShopApplication.Api
dotnet restore TestShopApplication.Api.csproj
start "" http://localhost:5000/swagger
dotnet run TestShopApplication.Api.csproj