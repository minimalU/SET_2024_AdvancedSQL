// FILE: ApiOperator.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-12-05
// DESCRIPTION: ApiHelper.cs provides opearations for api calls.
using Knot_AssemblyLine.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Knot_AssemblyLine
{
    public class ApiOperator
    {
        private static List<WorkstationJob> workstationBins_list = null;
        public static async Task<List<WorkstationJob>> GetWorkstationjob()
        {
            const string url = "https://localhost:7148/api/Manufacturing/GetWorkstationjob";

            try
            {
                HttpResponseMessage response = await ApiHelper.Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<List<WorkstationJob>>(jsonString);
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
