FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /app
COPY --from=build /app/out ./

COPY ./tests ./tests
WORKDIR /app/tests

RUN dotnet restore
RUN dotnet test
