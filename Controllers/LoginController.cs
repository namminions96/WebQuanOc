using CheckApiWeb.Models;
using Microsoft.AspNetCore.Mvc;
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
    
            if (field.Name != "" && field.Name != null)
            {
                HttpContext.Session.SetString("Name", field.Name);
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
    }
}
