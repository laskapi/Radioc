using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Radioc.Clients;
using Radioc.Models;

namespace Radioc.Controllers
{
    public class HomeController(ILogger<HomeController> logger,RadioBrowserClient client) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly RadioBrowserClient _radioBrowserClient = client;

        public async Task<IActionResult> Index(string SearchString)
        {

            var stations = await _radioBrowserClient.FindStationsAsync(SearchString);
            var stationsVM = new StationsVM
            {
                SearchString = SearchString,
                Stations = stations ?? Array.Empty<Station>()
            };

             return View(stationsVM);
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
