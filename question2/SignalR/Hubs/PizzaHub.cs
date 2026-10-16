using Microsoft.AspNetCore.SignalR;
using SignalR.Services;

namespace SignalR.Hubs
{
    public class PizzaHub : Hub
    {
        private readonly PizzaManager _pizzaManager;

        public PizzaHub(PizzaManager pizzaManager) {
            _pizzaManager = pizzaManager;
        }

        public override async Task OnConnectedAsync()
        {

            await base.OnConnectedAsync();
            _pizzaManager.AddUser();
            await Clients.All.SendAsync("UpdateNbUsers", _pizzaManager.NbConnectedUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            _pizzaManager.RemoveUser();
            await Clients.All.SendAsync("UpdateNbUsers", _pizzaManager.NbConnectedUsers);

        }

        public async Task SelectChoice(PizzaChoice choice)
        {
          var groupe = _pizzaManager.GetGroupName(choice);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupe);
            await Clients.Caller.SendAsync("UpdatePizzaPrice", _pizzaManager.GetPizzaPrice(choice));
            await Clients.Caller.SendAsync("UpdateNbPizzasAndMoney", _pizzaManager.GetNbPizzas(choice), _pizzaManager.GetMoney(choice));
        }

        public async Task UnselectChoice(PizzaChoice choice)
        {
        }

        public async Task AddMoney(PizzaChoice choice)
        {
        }

        public async Task BuyPizza(PizzaChoice choice)
        {
        }
    }
}
