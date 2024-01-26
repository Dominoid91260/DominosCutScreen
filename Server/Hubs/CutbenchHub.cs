using Microsoft.AspNetCore.SignalR;

namespace DominosCutScreen.Server.Hubs
{
    public class CutbenchHub : Hub
    {
        public async Task SetTimer(DateTime CreatedAt, TimeSpan Duration)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("SetTimer", CreatedAt, Duration);
        }

        public async Task PinOrder(int OrderNumber)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("PinOrder", OrderNumber);
        }

        public async Task DismissOrder(int OrderNumber)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("DismissOrder", OrderNumber);
        }
    }
}
