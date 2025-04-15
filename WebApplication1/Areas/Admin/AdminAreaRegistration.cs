using System.Web.Mvc;

namespace WebApplication1.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }


        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // Route để khi truy cập "Admin" thì tự động chuyển hướng về "Admin/Home/Index"
            context.MapRoute(
              "Admin_root",
              "Admin", // ✅ Đã thêm dấu phẩy
              new { controller = "Home", action = "Index" } // Điều hướng về HomeController trong Admin
          );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}