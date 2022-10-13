using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace AuthenticationInMVC.Models
{
    public class CustomAuthenticationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            // Check for authorization  
            if (HttpContext.Current.Session["User"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "UserLogins" },
                    { "action", "Login" }
               });
            }
        }
        //public void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    ulif (string.IsNlOrEmpty(Convert.ToString(filterContext.HttpContext.Session["User"])))
        //    {
        //        filterContext.Result = new HttpUnauthorizedResult();
        //    }
        //}

        //public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
        //    {
        //        //Redirecting the user to the Login View of Account Controller
        //        filterContext.Result = new RedirectToRouteResult(
        //       new RouteValueDictionary
        //       {
        //            { "controller", "UserLogins" },
        //            { "action", "Login" }
        //       });
        //    }
        //}
    }
}