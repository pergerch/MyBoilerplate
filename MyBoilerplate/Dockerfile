FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY MyBoilerplate/MyBoilerplate.csproj MyBoilerplate/
RUN dotnet restore MyBoilerplate/MyBoilerplate.csproj
COPY . .
WORKDIR /src/MyBoilerplate
RUN dotnet build MyBoilerplate.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MyBoilerplate.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyBoilerplate.dll"]
