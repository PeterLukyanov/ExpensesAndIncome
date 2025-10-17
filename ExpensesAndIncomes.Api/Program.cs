using Services;
using Microsoft.EntityFrameworkCore;
using Db;
using Repositorys;
using UoW;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File(
        "logs/webapi-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7
    )
    .CreateLogger();

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("ExpensesAndIncomesDb");
builder.Services.AddDbContext<ExpensesAndIncomesDb>(options =>
    options.UseSqlServer(connectionString));

/*var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

//Добавляем Аутентификацию
builder.Services.AddAuthentication(options =>
{
    //Определяем по какой схеме будет происходить проверка ключа
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //Определяем, что будет показыватся в случае провала аутентификации
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Здесь мы строим схему проверки ключа
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,

        ValidateAudience = false,
        //Проверять ли срок действия токена? если да - строгая сверка с сеттингс
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        //Проверяет подпись токена 
        ValidateIssuerSigningKey = true,    
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();*/

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

/*builder.Services.AddSwaggerGen(options =>
{
    //Определение типа аутентификации и настройка кнопки ауентификации
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {   
        //имя
        Name = "Authorization",
        //выбор типа схемы безопасности
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        //Название типа аутентификации
        Scheme = "Bearer",
        //Формат типа ключа
        BearerFormat = "JWT",
        //Где передаётся ключ(в данном случае токен), где его искать
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        //Что будет написано рядом с полем, в котором нужно вставить ключ
        Description = "Enter the token in the format: Bearer {your token}"
    });
    //Добавляет проверку токена на методах (в свагере отображается замки)
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            //Определяет какая схема используется для проверки токена
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    //Ссылка на схему безопасности 
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    //Показываем что мы ссылаемся на созданую в AddSecurityDefinition схему
                    Id = "Bearer"
                }
            },
            //Области доступа(ограничения), у меня пустой массив, потому что нет  ограничений  для Bearer схемы
            new string[] {}
        }
    });
});*/

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITypeOfIncomesRepository, TypeOfIncomesRepository>();
builder.Services.AddScoped<ITypeOfExpensesRepository, TypeOfExpensesRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IncomesTypeManipulator>();
builder.Services.AddScoped<IncomesManipulator>();
builder.Services.AddScoped<ExpensesTypesManipulator>();
builder.Services.AddScoped<ExpensesManipulator>();
builder.Services.AddScoped<TotalSumService>();

var app = builder.Build();

//app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseAuthentication();
//app.UseAuthorization();
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

//var usersService = services.GetRequiredService<IUserManagementService>();
//await usersService.LoadSuperUser();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();