namespace Radioc.Models
{
    public class StationsVM
    {
        public IEnumerable<Station> Stations { get; set; } = new List<Station>();

        public IEnumerable<FavoriteStation> Favorites { get; set; }=new List<FavoriteStation>();

        public string SearchString { get; set; } = "";
    }
}