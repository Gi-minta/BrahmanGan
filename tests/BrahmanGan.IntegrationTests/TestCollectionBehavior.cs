// Los tests de integración comparten la misma base de datos PostgreSQL, por lo que se
// desactiva la paralelización para evitar carreras al migrar/sembrar y al leer/escribir.
[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]
