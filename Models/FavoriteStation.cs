using Radioc.Areas.Identity.Data;

namespace Radioc.Models
{
    public class FavoriteStation
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string RadiocUserId { get; set; }
        public required RadiocUser RadiocUser { get; set; } 
    }
}
