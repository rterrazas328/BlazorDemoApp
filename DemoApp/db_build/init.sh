#!/bin/bash

set -e

if [[ -v RAILWAY_SERVICE_ID ]]; then
    echo "Railway Detected!"
else
    if [ -e /run/secrets/sa_password ]; then
        export SA_PASSWORD=$(cat /run/secrets/sa_password)
    fi
    if [ -e /run/secrets/db_password ]; then
        export DB_USER_PW=$(cat /run/secrets/db_password)
    fi
    if [ -e /run/secrets/db_app_user ]; then
        export DB_APP_USER=$(cat /run/secrets/db_app_user)
    fi
    if [ -e /run/secrets/db_app_password ]; then
        export DB_APP_PASSWORD=$(cat /run/secrets/db_app_password)
    fi
fi

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