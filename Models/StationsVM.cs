namespace Radioc.Models
{
    public class StationsVM
    {

        public required IEnumerable<Station> Stations { get; set; }

        public string SearchString { get; set; } = "";
    }
}