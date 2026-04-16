#Initial Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore




#Another layer for everything else
COPY . ./
RUN dotnet publish -c Release -o /app/publish


#Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Install SQLCMD (mssql-tools18)
RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl -sSL https://packages.microsoft.com/keys/microsoft.asc \
        | gpg --dearmor \
        | tee /usr/share/keyrings/microsoft-prod.gpg > /dev/null && \
    curl -sSL https://packages.microsoft.com/config/debian/12/prod.list \
        | sed 's#deb https://#deb [signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://#' \
        | tee /etc/apt/sources.list.d/mssql-release.list > /dev/null && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev && \
    ln -s /opt/mssql-tools18/bin/sqlcmd /usr/local/bin/sqlcmd





COPY --from=build /app/publish .

COPY entrypoint.sh /usr/local/bin/entrypoint.sh
COPY certs/aspnetapp.pfx /https/aspnetapp.pfx


RUN chmod +x /usr/local/bin/entrypoint.sh
RUN chmod +r /https/aspnetapp.pfx



ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]
