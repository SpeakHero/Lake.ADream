FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY Lake.ADream.sln ./
COPY Lake.ADream.Host/Lake.ADream.Host.csproj Lake.ADream.Host/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Lake.ADream.Host
RUN dotnet build Lake.ADream.Host.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Lake.ADream.Host.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Lake.ADream.Host.dll"]
