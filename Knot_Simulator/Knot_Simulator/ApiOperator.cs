// FILE: ApiOperator.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-25
// DESCRIPTION: ApiHelper.cs provides opearations for api calls.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Knot_Simulator
{
    public static class ApiOperator
    {
        private static List<Configuration> con_list = null;
        public static async Task<List<Configuration>> GetConfigList()
        {
            const string url = "https://localhost:7148/api/Manufacturing/GetConfigs";

            try
            {
                HttpResponseMessage response = await ApiHelper.Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<List<Configuration>>(jsonString);
                    con_list = obj;
                    return con_list;
                }
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Error: {e}");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error: GetConfigList Api response is unsuccessful, {ex}");
            }
            return null;
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

        public static async Task<int> PostInitialSimulationData(Configuration conf)
        {
            const string posturl = "https://localhost:7148/api/Manufacturing/AddSimulationData";
            try
            {
                HttpResponseMessage response = await ApiHelper.Client.PostAsJsonAsync(posturl, conf);
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
