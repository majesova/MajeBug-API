using MajeBugWebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace MajeBugWebApi.Controllers
{
    public class BaseApi :ApiController
    {

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager { get {
                _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                return _userManager;
            } }


        protected ApplicationUser CurrentUser { get {
                var claims = User as ClaimsPrincipal;
                var claimsThatContentsTheId = claims.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier);
                if (claimsThatContentsTheId.Count() > 0)
                {
                    var userId = claimsThatContentsTheId.SingleOrDefault().Value;
                    //query in UserManager
                    var user = UserManager.FindById(userId);
                    return user;
                }
                return null;
            } }

        protected string CurrentUserId { get {
                return CurrentUser != null ? CurrentUser.Id : null;
            } }

    }
}