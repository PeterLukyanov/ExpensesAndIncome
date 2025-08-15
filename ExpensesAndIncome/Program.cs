
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Models;
using Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
ListTypesOfIncomes listTypesOfIncomes = new ListTypesOfIncomes();
IncomesTypeManipulator incomesTypeManipulator = new IncomesTypeManipulator(listTypesOfIncomes);
ListTypesOfExpenses listTypesOfExpenses = new ListTypesOfExpenses();
ExpensesTypeManipulator expensesTypeManipulator = new ExpensesTypeManipulator(listTypesOfExpenses);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(listTypesOfIncomes);
builder.Services.AddSingleton(incomesTypeManipulator);
builder.Services.AddSingleton<IncomesManipulator>();
builder.Services.AddSingleton(listTypesOfExpenses);
builder.Services.AddSingleton(expensesTypeManipulator);
builder.Services.AddSingleton<ExpensesManipulator>();
builder.Services.AddSingleton<TotalSummService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();