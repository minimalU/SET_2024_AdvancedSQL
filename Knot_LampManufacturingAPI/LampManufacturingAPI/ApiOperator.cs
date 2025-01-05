// FILE: ApiOperator.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-25
// DESCRIPTION: ApiHelper.cs provides opearations for api calls.
using LampManufacturingAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;


namespace LampManufacturingAPI
{
    public static class ApiOperator
    {
        private static List<BinsWithWorkstationjob> binworkstation = null;

        public static async Task<int> RunBinSensor(string configid)
        {
            const string url = "https://localhost:7148/api/Manufacturing/ActivateBinSensor";

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
                throw new Exception($"Error: {e}");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error: GetConfigList Api response is unsuccessful, {ex}");
            }
        }

        public static async Task<int> RunBinButton(string configid)
        {
            const string url = "https://localhost:7148/api/Manufacturing/ActivateBinButton";

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
                throw new Exception($"Error: {e}");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error: GetConfigList Api response is unsuccessful, {ex}");
            }
        }

        public static async Task<List<BinsWithWorkstationjob>> GetBinWithWorkstation()
        {
            const string url = "https://localhost:7148/api/Manufacturing/GetViewBinWithWorkstation";

            try
            {
                HttpResponseMessage response = await ApiHelper.Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<List<BinsWithWorkstationjob>>(jsonString);
                    binworkstation = obj;
                    return binworkstation;
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
    }
}
