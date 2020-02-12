using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WebCCValidator.Interfaces;
using WebCCValidator.Models;

namespace WebCCValidator.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IChecker _checker;
        private readonly string NonNumericalRegex = "[^.0-9]";

        public HomeController(IConfiguration config, IChecker checker)
        {
           _checker = checker;         

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string creditCards)
        {
           // creditCards = Regex.Replace(creditCards, NonNumericalRegex, "");
            Dictionary<string, bool> CcDictionary =
            creditCards.Replace("\r", "").Split("\n").Select(item => item.Split('=')).ToDictionary(s => s[0], s => false); 

            foreach(KeyValuePair<string, bool> cc in CcDictionary)
            {
                CcDictionary[cc.Key] = _checker.CheckCC(cc.Key);
            }

            return View();
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
