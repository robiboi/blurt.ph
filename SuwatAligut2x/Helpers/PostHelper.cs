using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuwatAligut2x.Models;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Web.Mvc;

namespace SuwatAligut2x.Helpers
{
    public static class PostHelper
    {
        public static List<PostsModels> GetPostPage(int? page, int? size)
        {
            List<PostsModels> lstPost = new List<PostsModels>();
            PostsModels postModel = new PostsModels();
            try
            {
                if (page.HasValue && size.HasValue)
                    lstPost = postModel.GetPosts(10, 0);
                else
                    lstPost = postModel.GetPosts(10, 0);
            }
            catch
            {
                throw;
            }
            return lstPost;
        }

        public static string GetFriendlyOpenId(IAuthenticationResponse response, string email)
        {
            if (response.ClaimedIdentifier.ToString().Contains(WellKnownProviders.Google))
            {
                return "Google(" + email + ")";
            }
            if (response.ClaimedIdentifier.ToString().Contains(WellKnownProviders.Yahoo))
            {
                return "Yahoo(" + email + ")";
            }
            return response.FriendlyIdentifierForDisplay;
        }

        public static int GetUserId(string openId)
        {
            UsersModels user = new UsersModels();
            user = user.GetUserByOpenId(openId);
            if (user != null)
            {
                return user.UserId;
            }
            return 0;
        }

        public static MvcHtmlString LogonUser(this HtmlHelper htmlHelper)
        {
            UsersModels user = new UsersModels();
            string LoginUserDisplayName = user.GetUserByOpenId(HttpContext.Current.User.Identity.Name).DisplayName;

            TagBuilder anchor = new TagBuilder("a")
            {
                InnerHtml = LoginUserDisplayName
            };
            anchor.MergeAttribute("href", "");

            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
        }
    }
}
