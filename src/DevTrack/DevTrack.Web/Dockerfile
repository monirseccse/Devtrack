FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN apt-get update && apt-get install -y nodejs
COPY ["DevTrack.Web/*.csproj", "DevTrack.Web/"]
COPY ["DevTrack.Infrastructure/*.csproj", "DevTrack.Infrastructure/"]
RUN dotnet restore "DevTrack.Web/DevTrack.Web.csproj"
COPY . .
WORKDIR "/src/DevTrack.Web"
RUN dotnet build "DevTrack.Web.csproj" -c Release -o /app
RUN cp /src/wait-for-it.sh /app

FROM build AS publish
RUN dotnet publish "DevTrack.Web.csproj" -c Release -o /app
RUN cp /src/wait-for-it.sh /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
RUN chmod +x wait-for-it.sh
ENTRYPOINT ["dotnet", "DevTrack.Web.dll"]
