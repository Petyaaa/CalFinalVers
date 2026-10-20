using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SimpleCalculations.Models;

namespace SimpleCalculations.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string inputNumber, int fromBase, int toBase)
        {
            ViewBag.InputNumber = inputNumber;
            ViewBag.FromBase = fromBase;
            ViewBag.ToBase = toBase;

            if (string.IsNullOrWhiteSpace(inputNumber))
            {
                ViewBag.Error = "Моля, въведете число.";
                return View();
            }

            inputNumber = inputNumber.Trim().ToUpper();
            string allChars = "0123456789ABCDEF";

            int decimalValue = 0;
            int power = 1; 

            for (int i = inputNumber.Length - 1; i >= 0; i--)
            {
                char c = inputNumber[i];
                int charValue = allChars.IndexOf(c);

                if (charValue == -1 || charValue >= fromBase)
                {
                    ViewBag.Error = $"Невалиден символ '{c}' за избраната бройна система Base {fromBase}!";
                    return View();
                }

                decimalValue += charValue * power;
                power *= fromBase; 
            }

            if (toBase == 10)
            {
                ViewBag.Result = decimalValue.ToString();
                return View();
            }

            if (decimalValue == 0)
            {
                ViewBag.Result = "0";
                return View();
            }

            string finalResult = "";
            int tempNumber = decimalValue;

            while (tempNumber > 0)
            {
                int remainder = tempNumber % toBase;
                finalResult = allChars[remainder] + finalResult; 
                tempNumber /= toBase;
            }

            ViewBag.Result = finalResult;
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
