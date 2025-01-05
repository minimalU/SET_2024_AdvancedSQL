// FILE: ConfigOperator.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-18
// DESCRIPTION: ConfigOperator.cs provides the Api operation function for posting the configuration data to the endpoint.

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConfigurationTool
{
    public static class ConfigOperator
    {
        public static async Task<int> PostConfig(Configuration conf)
        {
            const string url = "https://localhost:7148/api/Manufacturing/AddConfiguration";

            try
            {
                HttpResponseMessage response = await ApiHelper.Client.PostAsJsonAsync(url, conf);
                if (response.IsSuccessStatusCode)
                {
                    return 0;
                }
                else
                {
                    throw new Exception("Error: PostConfig Api response is unsuccessful StatusCode");
                }
            }
            catch (HttpRequestException e)
            {
                return 1;
            }
            catch (Exception ex)
            {
                return 2;
            }
        }

        public static async Task<int> PostInitialSP(string configid)
        {
            const string url = "https://localhost:7148/api/Manufacturing/CallInitialSP";
            try
            {
                HttpResponseMessage response = await ApiHelper.Client.PostAsJsonAsync(url, configid);
                if (response.IsSuccessStatusCode)
                {
                    return 0;
                }
                else
                {
                    throw new Exception("Error: PostInitialSimulationData Api response is unsuccessful StatusCode");
                }
            }
            catch (HttpRequestException e)
            {
                return 1;
            }
            catch (Exception ex)
            {
                return 2;
            }
        }
    }
}