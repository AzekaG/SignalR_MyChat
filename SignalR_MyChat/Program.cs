using Microsoft.EntityFrameworkCore;
using SignalR_MyChat;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSignalR(); // регистрируем  сервис SignalR

builder.Services.AddDbContext<ChatContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<Controller>();

var app = builder.Build();


app.UseDefaultFiles();   //обработка запросов к wwwroot , подключается стартовая страница с нее
app.UseStaticFiles();  


app.MapHub<ChatHub>("/chat");   //middleware компонент, типизируем mapHub нашим Хабом , ("/chat) - маршрут -
// когда клиенты будут обращаться - они будут обращаться по етому маршрутую

app.Run();
