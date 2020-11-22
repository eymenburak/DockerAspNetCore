FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build 

WORKDIR /app

COPY *.csproj . 

RUN dotnet restore 

COPY . .

RUN dotnet publish DockerAspNetCore.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

COPY --from=build /app/out .

ENV SqlCon="LOCAL CONNECTION STRING"
ENTRYPOINT  ["dotnet","DockerAspNetCore.dll"]

