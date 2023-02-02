using Microsoft.AspNetCore.SignalR;
using signalrtest.Data;
using System.Security.Claims;

namespace signalrtest.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task SendMessageToReceiver(string sender, string receiver, string message)
        {
            var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;

            if (!string.IsNullOrEmpty(userId))
            {
                await Clients.User(userId).SendAsync("MessageReceived", sender, message);

            }

        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(userId))
            {
                var userName = _db.Users.FirstOrDefault(x => x.Id == userId);
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", userId, userName.UserName);

                HubConnections.AdduserConnection(userId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();

        }

        public override Task OnDisconnectedAsync(Exception? ex)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(HubConnections.HasUserConnection(userId, Context.ConnectionId))
            {
                var userConnections = HubConnections.Users[userId];
                userConnections.Remove(Context.ConnectionId);

                HubConnections.Users.Remove(userId);

                if(userConnections.Any())
                {
                    HubConnections.Users.Add(userId, userConnections);
                }
            }

            if (!String.IsNullOrEmpty(userId))
            {
                var userName = _db.Users.FirstOrDefault(x => x.Id == userId);
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisconnected", userId, userName.UserName);

                HubConnections.AdduserConnection(userId, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(ex);

        }

        public async Task SendAddRoomMessage (int maxRoom, int roomId, string roomName)
        {
            var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _db.Users.FirstOrDefault(x => x.Id == userId);

            await Clients.All.SendAsync("ReceiveAddRoomMessage", maxRoom, roomId, roomName, userId, userName);
        }

    }
}
