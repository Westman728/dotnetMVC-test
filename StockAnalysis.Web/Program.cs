using StockAnalysis.Web;

var builder = WebAppInitializer.CreateBuilder(args);
var app = builder.Build();
WebAppInitializer.ConfigureApp(app);

app.Run();