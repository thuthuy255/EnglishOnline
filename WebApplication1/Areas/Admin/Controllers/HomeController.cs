using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Users;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class HomeController : Controller
    {
        // GET: Admin/HomeƯ
        public ActionResult Index()
        {
            return View();
        }
    }
}