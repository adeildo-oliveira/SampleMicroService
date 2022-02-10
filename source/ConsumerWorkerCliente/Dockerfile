FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["source/ConsumerWorkerCliente/ConsumerWorkerCliente.csproj", "source/ConsumerWorkerCliente/"]
RUN dotnet restore "source/ConsumerWorkerCliente/ConsumerWorkerCliente.csproj"
COPY . .
WORKDIR "/src/source/ConsumerWorkerCliente"
RUN dotnet build "ConsumerWorkerCliente.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsumerWorkerCliente.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerWorkerCliente.dll"]

#docker build -t img-consumerservice-cliente -f .\source\ConsumerWorkerCliente\Dockerfile .
#docker run -d --name consumerservice-cliente img-consumerservice-cliente
#docker network connect laboratorio consumerservice-cliente
#
#docker rm consumerservice-cliente -f
#docker rmi img-consumerservice-cliente -f