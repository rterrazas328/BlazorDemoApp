#!/bin/bash


set -e

if [ -e /run/secrets/db_app_user ]; then
export DB_USER="$(cat /run/secrets/db_app_user)"
fi
if [ -e /run/secrets/db_app_password ]; then
export DB_PASSWORD="$(cat /run/secrets/db_app_password)"
fi

DB_HOST="db"

DB_NAME="AdventureWorksDW2025"

until /opt/mssql-tools18/bin/sqlcmd \
    -S "$DB_HOST" \
    -U "$DB_USER" \
    -P "$DB_PASSWORD" \
    -C \
    -Q "SELECT state_desc FROM sys.databases WHERE name='AdventureWorksDW2025'"  | grep -q "ONLINE" &>/dev/null
do
    echo "SQL Server not ready yet..."
    sleep 2
done

echo "SQL Server is online."


if [ -e /run/secrets/cert_password ]; then
export ASPNETCORE_Kestrel__Certificates__Default__Password="$(cat /run/secrets/cert_password)"
fi
#      - ASPNETCORE_Kestrel__Certificates__Default__Password=${cert_pwd}



if [ -e /run/secrets/connection_string ]; then
export ConnectionStrings__DemoAppContext=$(cat /run/secrets/connection_string)
fi
#	- ConnectionStrings__DemoAppContext=Server=db;Database=AdventureWorksDW2025;User=blazor_app;Password=${blazor_app};TrustServerCertificate=True



if [ -e /run/secrets/sa_password ]; then
export SA_PASSWORD=$(cat /run/secrets/sa_password)
fi
#      - SA_PASSWORD=${SA_PASSWORD}


if [ -e /run/secrets/jwt_token_key ]; then
export JWTTOKEN_KEY=$(cat /run/secrets/jwt_token_key)
fi
#      - JWTTOKEN_KEY=${JWTTOKEN_KEY}



echo "Starting .NET application..."
exec dotnet DemoApp.dll
