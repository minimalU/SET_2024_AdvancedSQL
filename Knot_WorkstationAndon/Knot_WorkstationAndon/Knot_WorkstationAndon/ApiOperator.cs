// FILE: ApiOperator.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-12-05
// DESCRIPTION: ApiHelper.cs provides opearations for api calls.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Knot_WorkstationAndon
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
