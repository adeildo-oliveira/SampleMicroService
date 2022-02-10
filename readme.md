>## Objetivo

A ideia neste Sample em micro serviços é, demonstrar o usos de tecnologias como GRPC, API, Worker, Kafka como messageria, Banco de dados (SQL SERVER) e por fim, container em docker.

Para facilitar, há um aquivo docker compose no qual é possível subir todos os serviços.

O kafka já está no docker compose como um serviço que pode ser subido como um container.

>## Fontes e Links para Subir Container Kafka e SQL Server

* [Apache Kafka - Docker](https://medium.com/azure-na-pratica/apache-kafka-kafdrop-docker-compose-montando-rapidamente-um-ambiente-para-testes-606cc76aa66)
* [Localhost kafka container](http://localhost:19000/)
* [SQL Server 2019 - Docker](https://docs.microsoft.com/pt-br/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash)

>### Criando uma Rede no Docker

``` ini
docker network create -d bridge laboratorio

```

Atribuir uma rede ao container: 
``` ini
docker network connect laboratorio IdContainer
```

>### Tópico
Assim que o serviço do kafka estiver online, será necessário criar um tópico para que os serviços possam conectar. No exemplo usado neste repo, foi criado tópico ```CLIENTE_ALTERAR``` como fila principal.

Basta acessar o link descrito como **Localhost kafka container**, clicar em **+New** e na caixa de texto que for exebida, inserir o nome do tópico e salvar.

Para casos onde teremos falhas, o tópico criado como deadletter é usado para cenários de falhas. O tópico criado no sample foi ```CLIENTE_ALTERAR_DEAD_LETTER```. 

>## Banco de Dados

``` ini
CREATE DATABASE SampleMicroService
GO

USE [SampleMicroService]
GO

CREATE TABLE [dbo].[Clientes](
	[Id] [uniqueidentifier] NOT NULL,
	[CpfCnpj] [varchar](20) NOT NULL,
	[Nome] [varchar](30) NOT NULL,
	[DataNascimento] [datetime] NULL,
	[Ativo] [bit] NOT NULL,
	[DataInclusao] [datetime] NULL,
	[DataAtualizacao] [datetime] NULL,
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

```