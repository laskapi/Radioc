using System.Text.Json.Serialization;

namespace Radioc.Models
{
    public class Station
    {

        required public string Name { get; set; }
        [JsonPropertyName("url_resolved")]
        required public string Url { get; set; }

        required public string Favicon { get; set; }    
    }
}