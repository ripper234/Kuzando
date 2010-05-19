using System;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using Kuzando.Common.Web;
using Kuzando.Model.Entities.DB;
using Kuzando.Persistence.Repositories;

namespace Kuzando.Web.Controllers
{
    public class AuthenticationController : UserAwareControllerBase
    {
        public AuthenticationController(IUserRepository userRepository)
            : base(userRepository)
        {
        }

        //
        // GET: /Authentication/

        public ActionResult Login()
        {
            return EmptyUserView();
        }

        public ActionResult LoginAsGuest()
        {
            return LoginAsUser("Guest", null);
        }

        //
        // Get: /Authentication/Login/openid
        public ActionResult Authenticate()
        {
            using (var relayingParty = new OpenIdRelyingParty())
            {
                var response = relayingParty.GetResponse();

                if (response == null)
                {
                    // Stage 2: user submitting Identifier
                    var openId = Request.Form["openid_identifier"];
                    var req = relayingParty.CreateRequest(openId);
                    req.AddExtension(new ClaimsRequest
                    {
                        Email = DemandLevel.Require,
                        FullName = DemandLevel.Require,
                        Nickname = DemandLevel.Request,
                    });
                    req.RedirectToProvider();

                    // todo - http://stackoverflow.com/questions/2724455/iauthenticationrequest-redirecttoprovider-is-not-supposed-to-return-yet-it-does
                    throw new Exception("Never gets here");
                }

                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var claimedIdentifier = response.ClaimedIdentifier;
                        return LoginAsUser(claimedIdentifier, response);

                    case AuthenticationStatus.Canceled:
                        ViewData["Message"] = "Canceled at provider";
                        // todo
                        return View("Login");

                    case AuthenticationStatus.Failed:
                        ViewData["Message"] = response.Exception.Message;
                        // todo
                        return View("Login");

                    default:
                        throw new Exception("Unknown status");
                }
            }
        }

        private ActionResult LoginAsUser(string openID, IAuthenticationResponse response)
        {
            var user = Users.FindByOpenId(openID);
            if (user != null)
            {
                // login
                return RedirectFromLoginPage(user);
            }

            // register
            if (response == null)
            {
                // handle non-open id (e.g. guest account)
                user = new User
                {
                    Name = "Guest",
                    OpenId = openID,
                    SignupDate = DateTime.Now
                };
                Users.Save(user);
                return RedirectFromLoginPage(user);
            }

            var sreg = response.GetExtension<ClaimsResponse>();
            if (sreg != null)
            {
                // todo (sreg has always been null when I tried to debug this)

                // the Provider MAY not provide anything
                // and even if it does, any of these attributes MAY be missing
                var email = sreg.Email;
                var fullName = sreg.FullName;
                // get the rest of the attributes, and store them off somewhere.
            }

            var username = FixUsername(response.FriendlyIdentifierForDisplay);
            user = new User
            {
                Name = username,
                OpenId = openID,
                SignupDate = DateTime.Now
            };
            Users.Save(user);
            return RedirectFromLoginPage(user);
        }

        private static string FixUsername(string username)
        {
            if (username == "www.google.com/accounts/o8/id")
                return "Unknown Google User";

            return username;
        }

        private ActionResult RedirectFromLoginPage(User user)
        {
            var returnUrl = Request.QueryString["ReturnURL"];
            switch (returnUrl)
            {
                case null:
                case "":
                case "/":
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), true); //not set cookie  
                    return RedirectToAction("Index", "Home");
            }

            FormsAuthentication.RedirectFromLoginPage(user.Id.ToString(), true);
            return new EmptyResult();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var returnUrl = Request.QueryString["ReturnUrl"];
            if (returnUrl != null)
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
