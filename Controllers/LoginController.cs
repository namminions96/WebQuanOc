using CheckApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Report.Data;

namespace CheckApiWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UseDbcontext _dbcontext;
        public LoginController(UseDbcontext dataDBcontext, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dataDBcontext;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index(login field)
        {
            var context = _httpContextAccessor.HttpContext;
            var user = _dbcontext.CreateUsers.FirstOrDefault(p=>  p.userName ==field.userName);
            if(user != null)
            {
                if(user.passWord == field.passWord)
                {
                    HttpContext.Session.SetString("Name", field.userName);
                    return RedirectToAction("Index", "Product");
                }
            }
            return View();
          
        }
        [HttpPost]
        public IActionResult Login(login field)
        {
            var context = _httpContextAccessor.HttpContext;
            var user = _dbcontext.Logins.Find(field.userName);
            if (user != null)
            {
                if (user.passWord == field.passWord)
                {
                    HttpContext.Session.SetString("Name", field.userName);
                    return RedirectToAction("Index", "Product");
                }
            }
            return View();

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateUser item)
        {
            try
            {
                CreateUser createUser = new CreateUser();
            
                createUser.userName = item.userName;
                createUser.passWord = item.passWord;
                createUser.soDienThoai = item.soDienThoai;
                createUser.Name = item.Name;
                _dbcontext.CreateUsers.Add(createUser);
                _dbcontext.SaveChanges();
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return View();
            }
        }
    }
}
