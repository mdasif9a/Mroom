using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRoom.Data;
using MRoom.Models;

namespace MRoom.Controllers
{
    public class AdminController : Controller
    {
        private readonly MRoomDbContext db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(MRoomDbContext dbContext, IWebHostEnvironment hostEnvironment)
        {
            db = dbContext;
            _hostingEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Settings(string OldPassword, string NewPassword)
        {
            UserLogin? login = db.UserLogins.Where(ur => ur.Role == "Admin").FirstOrDefault();
            if (login != null && login.Password == OldPassword)
            {
                login.Password = NewPassword;
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                TempData["datachange"] = "Your Password has Sucessfully Change";
            }
            else if (String.IsNullOrEmpty(OldPassword))
            {
                TempData["datachange"] = "Please Enter Your Old Password";
            }
            else
            {
                TempData["datachange"] = "Incorrect Old Passsword";
            }
            return RedirectToAction("Settings");
        }
    }
}
