var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
CryptoParser.ServicesContainer.Register(new CryptoParser.Parsing.ParsersManager());
CryptoParser.ServicesContainer.Register(new CryptoParser.Models.CVBsDataManager());
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
