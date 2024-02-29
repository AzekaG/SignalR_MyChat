using Microsoft.EntityFrameworkCore;
using SignalR_MyChat.Models;

namespace SignalR_MyChat
{
    public class ChatContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }



        public ChatContext(DbContextOptions<ChatContext> options) : base(options) 
        {
            if(Database.EnsureCreated())
            {
                Users.Add(new User { Name = "Admin" });
                Messages.Add(new Message { Messag = "Hello to All", Username = "Admin" });
                SaveChanges();
            }
        }

       public async Task CreateNewUser(User user)
        {
            if(user!=null) 
            {
                await Users.AddAsync(user);
                await SaveChangesAsync();
            }
        }

        public async Task CreateNewMessage(Message message)
        {
            if(message!=null)
            {
                await Messages.AddAsync(message);
                await SaveChangesAsync();
            }
        }

    }
}
