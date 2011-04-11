using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuwatAligut2x.Models;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using SuwatAligut2x.Helpers;
using System.Configuration;

namespace SuwatAligut2x.Controllers
{
    public class UsersController : Controller
    {
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        //
        // GET: /Users/

        [Authorize]
        public ActionResult Index(int id)
        {
            UsersModels user = new UsersModels();
            user.UserId = id;
            user = user.GetUser();
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Index(UsersModels user)
        {
            user.UpdateUser();
            return View(user);
        }

        #region Login Logic
        public ActionResult Logon()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthenticationService formAuth = new FormsAuthenticationService();
            formAuth.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult FacebookAuth(string returnUrl)
        {
            string appId = ConfigurationManager.AppSettings["AppId"];
            string facebookauth = ConfigurationManager.AppSettings["FacebookAuthURL"];
            string appsecret = ConfigurationManager.AppSettings["AppSecret"];
            
            // if code is not available, we should request some.
            if (Request.Params["code"] == null)
            {
                string code_url = @"https://www.facebook.com/dialog/oauth?client_id=" + appId + 
                    "&redirect_uri=" + Server.UrlEncode(facebookauth) + "&scope=email,read_stream";
                Response.Redirect(code_url);
            }
            else
            {
                string token_url = @"https://graph.facebook.com/oauth/access_token?client_id=" + appId +
                                    "&redirect_uri=" + facebookauth + "&client_secret=" + appsecret + "&code=" + Request.Params["code"];

                string tokenKeyValue = PostHelper.file_get_contents(token_url);
                string token = PostHelper.GetKeyValueFromString(tokenKeyValue, "access_token");

                Facebook.FacebookAPI api = new Facebook.FacebookAPI(token);

                Facebook.JSONObject me = api.Get("/me");

                UsersModels user = new UsersModels();

                // NOTE: 
                // api.AccessToken is temporary. It will be replaced to a 
                // more proper ClaimedOpenId or public profile for facebook. e.g. http://www.facebook.com/robiboi

                user = user.GetUserByOpenId(api.AccessToken);   // should be the identifier of the user in facebook, e.g. profile link.
                if (user == null)
                {
                    RegisterOpenId roi = new RegisterOpenId();
                    roi.ClaimedOpenId = api.AccessToken; // same as above
                    roi.FriendlyOpenId = api.AccessToken; // could be profile link.
                    roi.ReturnUrl = returnUrl;
                    roi.Email = null;
                    return View(roi);
                }

                FormsAuthenticationService formAuth = new FormsAuthenticationService();
                formAuth.SignIn(api.AccessToken, false);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return new EmptyResult();
        }

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            // handle oauth authentication
            if (string.IsNullOrEmpty(Request.Form["openid_identifier"]))
            {
                // handle oauth version 2.0
                if (Request.Form["oauth_version"] == "2.0")
                {
                    return FacebookAuth(returnUrl);
                }
            }

            var response = openid.GetResponse();
            
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        var request = openid.CreateRequest(Request.Form["openid_identifier"]);
                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        request.AddExtension(fetch);

                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewData["Message"] = ex.Message;
                        return View("Logon");
                    }
                }
                else
                {
                    ViewData["Message"] = "Invalid identifier";
                    return View("Logon");
                }
            }
            else
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:

                        UsersModels user = new UsersModels();

                        var fetch = response.GetExtension<FetchResponse>();
                        string email = null;
                        if (fetch != null)
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);

                        // for new OpenId
                        user = user.GetUserByOpenId(response.ClaimedIdentifier);
                        if (user == null)
                        {
                            RegisterOpenId roi = new RegisterOpenId();
                            roi.ClaimedOpenId = response.ClaimedIdentifier;
                            roi.FriendlyOpenId = PostHelper.GetFriendlyOpenId(response, email);
                            roi.ReturnUrl = returnUrl;
                            roi.Email = email;
                            return View(roi);
                        }

                        FormsAuthenticationService formAuth = new FormsAuthenticationService();
                        formAuth.SignIn(response.ClaimedIdentifier, false);

                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    case AuthenticationStatus.Canceled:
                        ViewData["Message"] = "Canceled at provider";
                        return View("Logon");
                    case AuthenticationStatus.Failed:
                        ViewData["Message"] = response.Exception.Message;
                        return View("Logon");
                }
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult OpenIdConfirm(RegisterOpenId openId)
        {
            UsersModels user = new UsersModels();
            user.CreateNewUser(openId.ClaimedOpenId, openId.FriendlyOpenId, openId.Email);
            
            FormsAuthenticationService formAuth = new FormsAuthenticationService();
            formAuth.SignIn(openId.ClaimedOpenId, false);

            if (!string.IsNullOrEmpty(openId.ReturnUrl))
                return Redirect(openId.ReturnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        #endregion

        public ActionResult Post(int? userid, int? msgid)
        {
            if (!msgid.HasValue)
            {
                List<PostsModels> userPosts = PostsModels.GetPostsByUser(userid.Value);
                // show all users post
                return View("UserPosts", userPosts);
            }
            else
            {
                PostsModels userPost = PostsModels.GetPostByMessageId(msgid.Value);
                // show specific user post
                return View(userPost);
            }
        }

        [HttpPost]
        public PartialViewResult AddComment(FormCollection form)
        {
            int msgId = Convert.ToInt32(form["MessageId"]);
            string comment = form["NewComment"];
            CommentsModels cm = new CommentsModels();
            cm.MessageId = msgId;
            cm.Comment = comment;
            cm.CommentorsId = PostHelper.GetUserId(HttpContext.User.Identity.Name);
            cm.CreateComment();

            List<CommentsModels> comments = PostsModels.GetPostByMessageId(msgId).Comments;
            return PartialView("Comments", comments);
        }
    }
}
