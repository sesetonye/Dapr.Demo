#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Subscriber.State/Subscriber.State.csproj", "Subscriber.State/"]
RUN dotnet restore "Subscriber.State/Subscriber.State.csproj"
COPY . .
WORKDIR "/src/Subscriber.State"
RUN dotnet build "Subscriber.State.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Subscriber.State.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subscriber.State.dll"]