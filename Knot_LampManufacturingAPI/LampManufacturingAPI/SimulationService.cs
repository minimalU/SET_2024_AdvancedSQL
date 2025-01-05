// Task for sending message to the hub

using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using System.Xml;
using Microsoft.AspNetCore.SignalR;
using LampManufacturingAPI.Hubs;
using Microsoft.Extensions.Logging;

namespace LampManufacturingAPI;

public class SimulationService : BackgroundService
{
    private static readonly TimeSpan t = TimeSpan.FromSeconds(5);
    private readonly ILogger<SimulationService> logger;
    private readonly IHubContext<MessageHub> hubContext;

    public SimulationService(ILogger<SimulationService> logger, IHubContext<MessageHub> hubContext)
    {
        this.logger = logger;
        this.hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(t);

        //while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        //{
        //    logger.LogInformation(DateTime.Now);
        //    Console.WriteLine(DateTime.Now.ToString());

        //    await hubContext.Clients.All.ReceiveMessage($"server time = {DateTime.Now}");
        //    await hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", $"{DateTime.Now}");
        //}
    }
}
