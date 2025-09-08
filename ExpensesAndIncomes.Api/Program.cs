using Services;
using Microsoft.EntityFrameworkCore;
using Db;
using Repositorys;
using UoW;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/webapi-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("ExpensesAndIncomesDb");
builder.Services.AddDbContext<ExpensesAndIncomesDb>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITypeOfIncomesRepository, TypeOfIncomesRepository>();
builder.Services.AddScoped<ITypeOfExpensesRepository, TypeOfExpensesRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IncomesTypeManipulator>();
builder.Services.AddScoped<IncomesManipulator>();
builder.Services.AddScoped<ExpensesTypesManipulator>();
builder.Services.AddScoped<ExpensesManipulator>();
builder.Services.AddScoped<TotalSumService>();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
// Use migrations and create database, if it does not exist(I create this for SQL in container)
using (var scope1 = app.Services.CreateScope())
{
    var dbContext = scope1.ServiceProvider.GetRequiredService<ExpensesAndIncomesDb>();
    dbContext.Database.Migrate();
}
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

//Here the program loads a couple of types of expenses and income if the database is empty
var expensesManipulator = services.GetRequiredService<ExpensesTypesManipulator>();
await expensesManipulator.LoadTypeOfExpenses();

var incomesManipulator = services.GetRequiredService<IncomesTypeManipulator>();
await incomesManipulator.LoadTypeOfIncomes();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();