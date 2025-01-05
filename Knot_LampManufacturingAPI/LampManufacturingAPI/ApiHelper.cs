using System.Net.Http;

namespace LampManufacturingAPI
{
    public static class ApiHelper
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
