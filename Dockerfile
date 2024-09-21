FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-webapi
WORKDIR /src
COPY ./src/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o /out/webapi

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-tests
WORKDIR /tests
COPY ./tests/ ./
COPY --from=build-webapi /src/ ../src/
RUN dotnet restore
RUN dotnet build -c Release -o /out/tests

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-webapi /out/webapi .
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "webapi.dll"]
