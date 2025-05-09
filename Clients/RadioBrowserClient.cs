using Radioc.Models;

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

        public async Task<IEnumerable<Station>?> FindStationsAsync(string search)=> 
            await _httpClient.GetFromJsonAsync<IEnumerable<Station>>("search?limit=10&name="+search);

    }
}
