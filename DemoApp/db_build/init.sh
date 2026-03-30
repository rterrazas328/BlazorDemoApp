#!/bin/bash

set -e

# Start SQL Server in background
/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to start..."

until /opt/mssql-tools18/bin/sqlcmd -S "localhost" -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" > /dev/null 2>&1
do
  sleep 5
done

echo "SQL Server is ready"

# Restore database
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "
  RESTORE DATABASE AdventureWorksDW2025
  FROM DISK = '/var/opt/mssql/backup/AdventureWorksDW2025_InitialDemoApp.bak'
  WITH MOVE 'AdventureWorksDW' TO '/var/opt/mssql/data/AdventureWorksDW2025.mdf',
       MOVE 'AdventureWorksDW_log' TO '/var/opt/mssql/data/AdventureWorksDW2025_log.ldf',
       REPLACE;
"

#Restore logins
/opt/mssql-tools18/bin/sqlcmd -S localhost -d AdventureWorksDW2025 -U sa -P "$SA_PASSWORD" -C -i /logins.sql

echo "Database ready.."

# Keep SQL Server running
wait