using LogLib;
using PIO.Models;
using PIO.DataProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<LogLib.ILogger, ConsoleLogger>( (serviceProvider) => new ConsoleLogger(new DefaultLogFormatter()) );

PIODatabase database = new PIODatabase();
database.PersonnTable.Add(new Personn() { PersonnID = 1, FirstName = "John", LastName = "Doe" });
IDataProvider dataProvider = new MemoryDataProvider(database);

builder.Services.AddSingleton(typeof(IDataProvider), dataProvider);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
