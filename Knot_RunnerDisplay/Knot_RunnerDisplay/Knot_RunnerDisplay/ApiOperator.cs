// FILE: ApiOperator.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-25
// DESCRIPTION: ApiHelper.cs provides opearations for api calls.

using Knot_RunnerDisplay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Knot_RunnerDisplay
{
    internal class ApiOperator
    {
        private static List<BinWithWorkstationjob> workstationBins_list = null;
        public static async Task<List<BinWithWorkstationjob>> GetViewBinWithWorkstation()
        {
            const string url = "https://localhost:7148/api/Manufacturing/GetViewBinWithWorkstation";

            try
            {
                HttpResponseMessage response = await ApiHelper.Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<List<BinWithWorkstationjob>>(jsonString);
                    workstationBins_list = obj;
                    return workstationBins_list;
                }
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"Error: {e}");
            }

            catch (Exception ex)
            {
                throw new Exception($"Error: GetViewWithWorkstation Api response is unsuccessful, {ex}");
            }
            return null;
        }
    }
}
