using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(WebApplication1.Startup))]

namespace WebApplication1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 🔹 Đặt mặc định SignIn Authentication Type
            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ExternalCookie);

            // 🔹 Đăng ký middleware Cookie Authentication trước
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromDays(30),
                SlidingExpiration = true
            });

            // 🔹 Đăng ký middleware External Authentication
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 🔹 Đăng ký Google Authentication
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "425813942861-78mvincqnmdinj0fbrc8vm162gil1aim.apps.googleusercontent.com",
                ClientSecret = "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",
                CallbackPath = new PathString("/signin-google") // Phải trùng với Google Console
            });
        }
    }
}
