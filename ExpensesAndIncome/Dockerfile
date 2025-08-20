# Сборка проекта
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем файлы проекта и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем весь код и публикуем
COPY . ./
RUN dotnet publish -c Release -o out

# Лёгкий образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Задаём порты и переменные окружения
ENV ASPNETCORE_URLS="http://+:5119;https://+:7231"

EXPOSE 5119
EXPOSE 7231

ENTRYPOINT ["dotnet", "ExpensesAndIncome.dll"]