// FILE: MessageHub.cs
// PROJECT : PROG3070-Proejct-Knot
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-18
// DESCRIPTION: SignalRHub - the server
// https://www.youtube.com/watch?v=RaXx_f3bIRU
// https://www.youtube.com/watch?v=O7oaxFgNuYo

using LampManufacturingAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace LampManufacturingAPI.Hubs
{
    public class MessageHub: Hub
    {
        private readonly IHubContext<MessageHub> hubContext;

        public MessageHub(IHubContext<MessageHub> hubctx)
        {
            hubContext = hubctx;
        }

        public async Task SendMessage(string user, string message)
        {
            // if a message from the simulator, API call for the simulation operation
            if (user == "Simulator")
            {
                
                var msg = message.Split('='); //msg[0] = timername, msg[1]=configurationid
                if (msg.Length > 0)
                {
                    if (msg[0] == "bin")
                    {
                        // sensor - reduce the count bin by 1
                        var result = await ApiOperator.RunBinSensor(msg[1]);
                    }
                    if (msg[0] == "runner")
                    {
                        if (msg[2] == "elapsed")
                        {
                            // replenishment
                            var result = await ApiOperator.RunBinButton(msg[1]);
                            //Console.WriteLine(result);
                        }
                    }
                    if (msg[0] == "station")
                    {
                        // simulation time ends up
                    }
                }
                Console.WriteLine($"MessageHub:SendMessage-user-{user} message-{message}");
            }

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
