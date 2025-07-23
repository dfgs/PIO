using LogLib;
using PIO.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<LogLib.ILogger, ConsoleLogger>( (serviceProvider) => new ConsoleLogger(new DefaultLogFormatter()) );

PIODatabase database = new PIODatabase();
database.PersonnTable.Add(new Personn() { PersonnID = 1, FirstName = "John", LastName = "Doe" });
builder.Services.AddSingleton(typeof(PIODatabase),database);


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
