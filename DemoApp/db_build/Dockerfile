FROM mcr.microsoft.com/mssql/server:2025-latest AS build

USER root

COPY ./init.sh /init.sh
Copy ./logins.sql /logins.sql

RUN chmod +x /init.sh
RUN chmod +x /logins.sql


USER mssql


ENTRYPOINT ["/init.sh"]