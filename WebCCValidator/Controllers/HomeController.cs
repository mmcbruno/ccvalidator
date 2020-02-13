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
            Dictionary<string, bool> CcDictionary = new Dictionary<string, bool>();
            IEnumerable<string> numbers = creditCards.Trim().Split('\n').ToList().Distinct();
            foreach (string cc in numbers) 
            {
                CcDictionary.Add(cc.Trim(),_checker.CheckCC(cc.Trim()));
            }

            return View(CcDictionary);
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
