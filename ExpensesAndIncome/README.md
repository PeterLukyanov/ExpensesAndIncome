# ExpensesAndIncomes project
This project provides a REST API for managing incomes and expenses.

# Technologies
- C#
- .NET 9.0
- ASP.NET Core
- Entity Framework 
- Microsoft SQL
- Docker
- Unit tests
- LINQ

# Installation and launch
1.Download Docker from the official website: https://www.docker.com/

2.After installing Docker, run the following commands in the command line:

    1) docker pull petrlukyanov/my-rest-api:latest

    2) docker run -d -p 5119:5119 --name my-rest-api -e ASPNETCORE_URLS="http://+:5119" -e ASPNETCORE_ENVIRONMENT=Development petrlukyanov/my-rest-api:latest

3.Enter the following address into your browser: 

http://localhost:5119/swagger/index.html

All information will be stored in a SQL database in one of the containers.