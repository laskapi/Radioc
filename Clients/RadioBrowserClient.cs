using Microsoft.IdentityModel.Tokens;
using Radioc.Models;
using System.Net.Http.Json;

namespace Radioc.Clients

{
    public class RadioBrowserClient
    {
        private readonly HttpClient _httpClient;

        public RadioBrowserClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://162.55.180.156/json/stations/");

        }

        public async Task<IEnumerable<Station>?> FindStationsAsync(string search)
        {

            var queryString = "search?limit=9&order=votes";
            if (!search.IsNullOrEmpty())
            {
                queryString+= "&name="+search;
            }
          return await _httpClient.GetFromJsonAsync<IEnumerable<Station>>( queryString);
        }
     


    }
}
