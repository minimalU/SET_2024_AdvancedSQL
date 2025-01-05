// FILE: ApiHelper.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-12-05
// DESCRIPTION: ApiHelper.cs initializes HttpClient for an API call.
// REFERENCE: AUTHOR: IAmTimCorey https://www.youtube.com/watch?v=aWePkE2ReGw&t=249ss

using System.Net.Http;

namespace Knot_WorkstationAndon
{
    internal class ApiHelper
    {
        public static HttpClient Client { get; set; } = new HttpClient();
        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
