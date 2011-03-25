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

namespace SuwatAligut2x.Controllers
{
    public class UsersController : Controller
    {
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        //
        // GET: /Users/

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

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
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
            user.CreateNewUser(openId.ClaimedOpenId, openId.FriendlyOpenId);
            
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
