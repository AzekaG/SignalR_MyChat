using Microsoft.EntityFrameworkCore;
using SignalR_MyChat;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSignalR(); // ������������  ������ SignalR

builder.Services.AddDbContext<ChatContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<Controller>();

var app = builder.Build();


app.UseDefaultFiles();   //��������� �������� � wwwroot , ������������ ��������� �������� � ���
app.UseStaticFiles();  


app.MapHub<ChatHub>("/chat");   //middleware ���������, ���������� mapHub ����� ����� , ("/chat) - ������� -
// ����� ������� ����� ���������� - ��� ����� ���������� �� ����� ���������

app.Run();
