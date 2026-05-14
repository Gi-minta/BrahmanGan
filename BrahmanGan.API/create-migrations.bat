@echo off
echo Creando migracion de base de datos principal...
dotnet ef migrations add InitialCreate --project ..\BrahmanGan.Infrastructure --startup-project .
echo.
echo Creando migracion de Event Store...
dotnet ef migrations add InitialEventStore --context EventStoreDbContext --project ..\BrahmanGan.Infrastructure --startup-project .
echo.
echo Migraciones creadas exitosamente!
pause
