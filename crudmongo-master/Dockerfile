FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

ARG EstudiantesDatabaseSettings__ConnectionString

# copy csproj and restore as distinct layers
#COPY *.sln .
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build app
COPY ./. ./
WORKDIR /app/
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "aspnetdemo2.dll"]