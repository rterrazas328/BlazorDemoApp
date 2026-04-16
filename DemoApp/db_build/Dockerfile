FROM mcr.microsoft.com/mssql/server:2025-latest AS build

USER root

COPY ./init.sh /init.sh
Copy ./logins.sql /logins.sql
Copy ./backups/AdventureWorksDW2025_InitialDemoApp.bak /var/opt/mssql/backup/


RUN chmod +x /init.sh
RUN chmod +x /logins.sql
RUN chmod +r /var/opt/mssql/backup/AdventureWorksDW2025_InitialDemoApp.bak


USER mssql


ENTRYPOINT ["/init.sh"]