using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cineflex.Services
{
    public interface IIpService
    {
        Task<JsonElement> LookupIpIpApi(string ip);
    }

        public class IpService : IIpService
        {
        public async Task<JsonElement> LookupIpIpApi(string ip)
        {
            try
            {
                using var http = new HttpClient();
                // ip-api free: http (pro supports https). Voor productie: gebruik pro/HTTPS.
                string url = $"http://ip-api.com/json/{ip}?fields=status,message,country,regionName,city,zip,lat,lon,isp,query";
                var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.GetProperty("status").GetString() == "success")
                {
                    return root.Clone();
                }
                else
                {
                    throw new Exception("IP failed: " + root.GetProperty("message").GetString());
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }

            
        }
    }
     
}


