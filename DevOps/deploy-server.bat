cd ../TestShopApp-Api/TestShopApplication.Api
set baseFolder=%cd%
xcopy "%baseFolder%\APP_DATA\TestShopApp.db*" "%baseFolder%\..\www\APP_DATA\TestShopApp.db*" /Y
dotnet restore
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=True -o "../www"
REM tar.exe -zcvf "%baseFolder%\..\www\TestShopApp.gz" "%baseFolder%\..\www\*"