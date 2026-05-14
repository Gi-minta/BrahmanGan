@echo off
echo Aplicando migraciones...
echo.
echo [1/2] Aplicando migracion de base de datos principal...
dotnet ef database update --project ..\BrahmanGan.Infrastructure --startup-project .
echo.
echo [2/2] Aplicando migracion de Event Store...
dotnet ef database update --context EventStoreDbContext --project ..\BrahmanGan.Infrastructure --startup-project .
echo.
echo Migraciones aplicadas exitosamente!
pause
