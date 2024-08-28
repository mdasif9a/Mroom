using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MRoom.Models;
using System.Diagnostics;
using System.Security.Claims;
using MRoom.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly MRoomDbContext db;
        public HomeController(MRoomDbContext dbContext)
        {
            db = dbContext;
        }
        public IActionResult Index()
        {
            ViewBag.CommercialShop = db.PropertyDetails.AsNoTracking().Where(x => x.PropertyVariantName == "SHOP").Take(8).ToList();
            ViewBag.BHK1 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "1 BHK").Take(8).ToList();
            ViewBag.BHK2 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "2 BHK").Take(8).ToList();
            ViewBag.BHK3 = db.PropertyDetails.AsNoTracking().Where(x => x.BHKTypeName == "3 BHK").Take(8).ToList();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TermsOfUse()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult PropertyList(string Name = "")
        {
            return View();
        }

        public IActionResult PropertyDetails(int Pid = 0)
        {
            return View();
        }

        public IActionResult Testimonials()
        {
            return View();
        }

        public IActionResult Tenats()
        {
            return View();
        }

        public IActionResult OwnerLandlords()
        {
            return View();
        }

        public IActionResult ListingWhatsapp()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Username, string Password, string ReturnUrl)
        {
            UserLogin? myuser = await db.UserLogins.FirstOrDefaultAsync(x => x.Username == Username && x.Password == Password);
            if (myuser != null)
            {
                if (myuser.Role == "User" || myuser.Role == "Admin")
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, myuser.Username),
                        new Claim(ClaimTypes.Role, myuser.Role)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }

                    if (myuser.Role == "User")
                    {
                        return RedirectToAction("Index", "MyUser");
                    }
                    else if (myuser.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                else
                {
                    TempData["datachange"] = "Acoount Not Active Contact Admin For Activate Your Account..";
                }
            }
            else
            {
                TempData["datachange"] = "Incorrect username or password.";
            }
            return RedirectToAction("Login", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
