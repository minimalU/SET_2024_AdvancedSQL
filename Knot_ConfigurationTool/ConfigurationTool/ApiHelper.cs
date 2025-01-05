// FILE: ApiHelper.cs
// PROJECT : PROG3070 - Proejct
// PROGRAMMER: Yujung Park
// FIRST VERSION : 2024-11-18
// DESCRIPTION: ApiHelper.cs initializes HttpClient for an API call.

using System.Net.Http;

namespace ConfigurationTool
{
    public static class ApiHelper
    {
        public static HttpClient Client { get; set; } = new HttpClient();
        public static void InitializeClient()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
