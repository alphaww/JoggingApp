#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JoggingApp.EmailService/JoggingApp.EmailService.csproj", "JoggingApp.EmailService/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
COPY ["EventBus/EventBus.csproj", "EventBus/"]
COPY ["JoggingApp.IntegrationEvents/JoggingApp.IntegrationEvents.csproj", "JoggingApp.IntegrationEvents/"]
COPY ["JoggingApp.Core/JoggingApp.Core.csproj", "JoggingApp.Core/"]
RUN dotnet restore "JoggingApp.EmailService/JoggingApp.EmailService.csproj"
COPY . .
WORKDIR "/src/JoggingApp.EmailService"
RUN dotnet build "JoggingApp.EmailService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JoggingApp.EmailService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JoggingApp.EmailService.dll"]