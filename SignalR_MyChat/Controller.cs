using Microsoft.EntityFrameworkCore;
using SignalR_MyChat;

namespace SignalR_MyChat
{

    //добавить обработку контроллера!
    public class Controller
    {
        ChatContext chatContext;
        public Controller(ChatContext chatContext) 
        {
            this.chatContext = chatContext;
        }

        public async Task CreateNewUser(string name)
        {
            var us = (await chatContext.Users.ToListAsync()).Where(u => u.Name == name);
            if(us != null) return;
            await chatContext.CreateNewUser(new Models.User { Name = name });
        }



        public async Task CreateNewMessage(string username, string message)
        {
           await chatContext.CreateNewMessage(new Message { Messag = message, Username = username });
        }

        public async Task<List<Message>> getAllMessages()
        {
            return await chatContext.Messages.ToListAsync();
        }







    }
}
