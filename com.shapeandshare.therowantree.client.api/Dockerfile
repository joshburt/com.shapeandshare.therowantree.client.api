FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY com.shapeandshare.therowantree.client.api/com.shapeandshare.therowantree.client.api.csproj com.shapeandshare.therowantree.client.api/
RUN dotnet restore com.shapeandshare.therowantree.client.api/com.shapeandshare.therowantree.client.api.csproj
COPY . .
WORKDIR /src/com.shapeandshare.therowantree.client.api
RUN dotnet build com.shapeandshare.therowantree.client.api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish com.shapeandshare.therowantree.client.api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "com.shapeandshare.therowantree.client.api.dll"]
