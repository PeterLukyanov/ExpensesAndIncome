using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Models;
using Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ListTypesOfIncomes>();
builder.Services.AddSingleton<IncomesTypeManipulator>();
builder.Services.AddSingleton<IncomesManipulator>();
builder.Services.AddSingleton<ListTypesOfExpenses>();
builder.Services.AddSingleton<ExpensesTypesManipulator>();
builder.Services.AddSingleton<ExpensesManipulator>();
builder.Services.AddSingleton<TotalSummService>();

var app = builder.Build();
var incomesManipulator = app.Services.GetRequiredService<IncomesTypeManipulator>();
incomesManipulator.LoadTypeOfIncomes();

var expensesManipulator = app.Services.GetRequiredService<ExpensesTypesManipulator>();
expensesManipulator.LoadTypeOfExpenses();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();