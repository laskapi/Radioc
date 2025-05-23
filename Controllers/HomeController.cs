using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Radioc.Areas.Identity.Data;
using Radioc.CastingUtils;
using Radioc.Clients;
using Radioc.Data;
using Radioc.Models;

namespace Radioc.Controllers
{
    //  [Authorize]
    public class HomeController(ILogger<HomeController> logger, RadioBrowserClient client,
        ApplicationDbContext dbContext, UserManager<RadiocUser> userManager, MetaReaderService mReader) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly RadioBrowserClient _radioBrowserClient = client;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly UserManager<RadiocUser> _userManager = userManager;
        private readonly MetaReaderService mReader = mReader;

        public async Task<IActionResult> Index(string SearchString)
        {

            var stations =(await _radioBrowserClient.FindStationsAsync(SearchString))?.GroupBy(s=>s.Url).Select(y=>y.First());



            var stationsVM = new StationsVM
            {
                Stations = stations ?? Array.Empty<Station>(),
                SearchString = SearchString
            };

            var identity = User.Identity != null ? User.Identity.IsAuthenticated : false;
            ViewBag.LoggedIn = false;

            if (identity)
            {
                var id = _userManager.GetUserId(User);
                var favorites = from f in _dbContext.FavoriteStations
                                where f.RadiocUserId == id
                                select f;
                stationsVM.Favorites = await favorites.ToListAsync();

                ViewBag.LoggedIn = true;
            }
            _logger.LogInformation("favorite stations count: " + stationsVM.Favorites.Count());
            return View("Index", stationsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string searchString, string url, string favicon, string name)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {

                var IsAddedAlready = !(from f in _dbContext.FavoriteStations
                                       where f.Url == url && f.RadiocUserId == user.Id
                                       select f).IsNullOrEmpty();

                if (!IsAddedAlready)
                {
                    string verifiedIcon=favicon?[^4..]?? "";
                    if (verifiedIcon==".png" ||verifiedIcon==".jpg" || verifiedIcon==".ico" || verifiedIcon == ".svg")
                    {
                        verifiedIcon = favicon!;
                    }
                    else
                    {
                        verifiedIcon = "";
                    }
                      

                    var favorite = new FavoriteStation { Name = name, Url = url, Favicon = verifiedIcon, RadiocUserId = user.Id, RadiocUser = user };
                    var result = await _dbContext.FavoriteStations.AddAsync(favorite);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Added Radio: " + result.ToString());
                }
            }

            return await Index(searchString);

        }
        public async Task<IActionResult> Delete(string searchString, string name)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var resolvedStations = (from f in _dbContext.FavoriteStations
                                        where f.Name == name && f.RadiocUserId == user.Id
                                        select f).ToHashSet();



                if (resolvedStations.Count > 0)
                {

                    _dbContext.FavoriteStations.RemoveRange(resolvedStations);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("Removed Radios: " + resolvedStations.ToString());
                }
            }
            return await Index(searchString);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
