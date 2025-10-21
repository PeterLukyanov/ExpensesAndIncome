# ExpensesAndIncomes project
This project provides a REST API for managing incomes and expenses.

# Technologies and tools
- C#;
- CSS, HTML, JavaScript;
- .NET 9.0;
- ASP.NET Core;
- REST API;
- Entity Framework;
- Microsoft SQL;
- async/await;
- Unit tests;
- LINQ;
- Docker;
- Git, Github;
- CI/CD.

# Installation and launch
1.Download Docker from the official website: https://www.docker.com/

2.After installing Docker, run the following commands in the command line:

    1) docker pull petrlukyanov/my-rest-api:latest
    2) docker network create my-network
    3) docker run -d --name my-sql-server --network my-network -e "SA_PASSWORD=PasswordForSQL12345" -e "ACCEPT_EULA=Y" mcr.microsoft.com/mssql/server:2022-latest
    4) docker run -d --name my-rest-api --network my-network -p 5119:5119 -e ASPNETCORE_URLS="http://+:5119" -e ASPNETCORE_ENVIRONMENT=Development petrlukyanov/my-rest-api:latest

3.Enter the following address into your browser: 

http://localhost:5119/index.html

All information will be stored in a SQL database in one of the containers.

4. Shut down and delete all containers and network:

    1) docker stop my-rest-api
    2) docker stop my-sql-server
    3) docker rm my-rest-api
    4) docker rm my-sql-server
    5) docker network rm my-network
