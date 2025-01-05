using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.AspNetCore.SignalR.Client;

namespace Knot_Simulator
{
    public class SimulationOperator
    {
        private static Configuration targetconfig;
        public static async Task RunSimulator(HubConnection conn, Configuration target_config)
        {
            targetconfig = target_config ?? throw new ArgumentNullException(nameof(target_config));
            try
            {
                while (true)
                {
                    conn.InvokeAsync("SendMessage", "Simulator", $"SimulationOperator-{target_config.ConfigurationId}-RunSimulator");
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
