using Microsoft.AspNetCore.SignalR;
using SignalR_MyChat.Models;

namespace SignalR_MyChat
{

    //hub- базовый класс - ключевая сущность, через которую клиенты обмениваются сообщениями с сервером и между собой.
    public class ChatHub : Hub
    {
        Controller controller;
        public ChatHub(Controller controller)
        {
            this.controller = controller;
        }

        static List<User> _users = new List<User>();

        //когда клиент хочет написать сообщение в чат - то етот клиент вызывает у хаба етот метод, передавая свое имя и текст сообщения.
        public async Task Send(string username, string message)
        {
            await Clients.All.SendAsync("AddMessage", username, message);   //свойство клиентс - свойство базового класса.Всем_клиентам.Отправить("Функция у каждого клиента на клиентской стороне (index.html)", имя клиента, сообщение) 
            await controller.CreateNewUser(username);
            await controller.CreateNewMessage(username, message);

        }
        //когда клиент заходит впервые в чат - он должен закконектиться.B js на клиентской стороне вызывается метод Connect
        public async Task Connect(string userName)
        {
            List<Message> _messages = await controller.getAllMessages();
            var id = Context.ConnectionId; // как только вызвался метод Conntect - можно получить уикальный идентификатор, и у каждого клиента он свой. генерируется автоматически.

            if (!_users.Any(x => x.ConnectionId == id)) //если нашего айди нет в коллекции - 
            {


                await Clients.Caller.SendAsync("Connected", id, userName, _users); //как только клиент приконнектился- вызываем на клиентской стороне метод коннект. _users отправляется для того,
                                                                                   //чтобы клиент знал кто есть в чате.
                _users.Add(new User { ConnectionId = id, Name = userName });
                await controller.CreateNewUser(userName);//Caller означает, что мы отправляем сообщение тому клиенту, который законнектился.
                await Clients.Caller.SendAsync("History",_messages);
                //отправляем сообщеие всем , кроме того, кто только что закконектился
                await Clients.AllExcept(id).SendAsync("NewUserConnected", id, userName);
            }
        }


        //когда клиент открывает клиент , выходит из чата - срабатывает уничтожение сессии и срабатывает метод разрыва соединения.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var item = _users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (item != null)  //если юзер отключился  и он есть в списке - удаляем етого юзера.
            {
                _users.Remove(item);
                var id = Context.ConnectionId;

                await Clients.All.SendAsync("UserDisconnected", id, item.Name);         //отправляем всем сообщение , что юзер отключился.
            }
            await base.OnDisconnectedAsync(exception);
        }



    }
}
