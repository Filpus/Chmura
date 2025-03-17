using Aplication4;



RunPublisher runPublisher = new RunPublisher();
runPublisher.Main(args);

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
