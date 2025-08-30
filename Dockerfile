# Assembling the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copying project files and restoring dependencies
COPY ExpensesAndIncomes.Api/*.csproj ./ExpensesAndIncomes.Api/
COPY ExpensesAndIncomes.Tests/*.csproj ./ExpensesAndIncomes.Tests/
RUN dotnet restore ./ExpensesAndIncomes.Api/ExpensesAndIncomes.Api.csproj

# Copy the entire code and publish it
COPY . ./
RUN dotnet publish -c Release -o out

# Lightweight image to run
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Set ports and environment variables
ENV ASPNETCORE_URLS="http://+:5119;"

EXPOSE 5119

ENTRYPOINT ["dotnet", "ExpensesAndIncomes.Api.dll"]