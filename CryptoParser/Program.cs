using CryptoParser.Services;
using CryptoParser.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ServicesContainer.Register(new ParserManager());
ServicesContainer.Register(new Logger());
ServicesContainer.Register(new GoogleSheetFiller());
builder.Services.AddHostedService<TimedParserService>();
log4net.Config.XmlConfigurator.Configure(configFile: new FileInfo("log4net.config"));

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
