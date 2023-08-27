FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN mkdir -p /app/{Server,Client,Shared}
COPY Server/DominosCutScreen.Server.csproj /app/Server/
COPY Client/DominosCutScreen.Client.csproj /app/Client/
COPY Shared/DominosCutScreen.Shared.csproj /app/Shared/
RUN cd /app/ \
 && dotnet restore Server/DominosCutScreen.Server.csproj
COPY . /app
RUN cd /app/Server \
 && dotnet build DominosCutScreen.Server.csproj -c Release -o /app/build \
 && dotnet publish DominosCutScreen.Server.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as final
copy --from=build /app/publish /app
EXPOSE 80
EXPOSE 443
WORKDIR /app
ENTRYPOINT ["dotnet", "DominosCutScreen.Server.dll"]
