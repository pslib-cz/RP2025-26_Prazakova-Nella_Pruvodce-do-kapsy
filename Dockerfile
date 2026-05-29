FROM node:20-alpine AS client-build

WORKDIR /app/pruvodce.client

COPY pruvodce.client/package*.json ./
RUN npm ci

COPY pruvodce.client/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS server-build

WORKDIR /app/pruvodce.server

COPY pruvodce.server/*.csproj ./
RUN dotnet restore

COPY pruvodce.server/ ./

COPY --from=client-build /app/pruvodce.server/wwwroot/app ./wwwroot/app

RUN dotnet publish -c Release -o /publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

RUN mkdir -p /data

COPY --from=server-build /publish ./

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "pruvodce.server.dll"]