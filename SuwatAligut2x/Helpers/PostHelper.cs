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

        public static string GetRandomString(int length)
        {
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
            Random rand = new Random();
            
            string randomString = "";
            for (int i = 0; i < length; i++)
            {
                int index = rand.Next(letters.Length);
                randomString += letters[index].ToString();
            }

            return randomString;
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
            user = user.GetUserByOpenId(HttpContext.Current.User.Identity.Name);
            string LoginUserDisplayName = user.DisplayName;

            string anc = HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, 
                htmlHelper.RouteCollection, LoginUserDisplayName, "", "Index", "Users", 
                new System.Web.Routing.RouteValueDictionary(new { id = user.UserId }), null);

            return MvcHtmlString.Create(anc);
        }

        public static string file_get_contents(string fileName)
        {
            string sContents = string.Empty;
            if (fileName.ToLower().IndexOf("http:") > -1 || fileName.ToLower().IndexOf("https:") > -1)
            { // URL 
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] response = wc.DownloadData(fileName);
                sContents = System.Text.Encoding.ASCII.GetString(response);
            }
            else
            {
                // Regular Filename 
                System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                sContents = sr.ReadToEnd();
                sr.Close();
            }
            return sContents;
        }

        public static List<KeyValuePair<string, string>> StringToKeyValuePair(string keyValueStr)
        {
            string[] strSplit = keyValueStr.Split('&');
            List<KeyValuePair<string, string>> lstKeyValuePair = new List<KeyValuePair<string,string>>();

            foreach (string str in strSplit)
            {
                string[] s = str.Split('=');
                lstKeyValuePair.Add(new KeyValuePair<string, string>(s[0], s[1]));
            }
            return lstKeyValuePair;
        }

        public static string GetKeyValueFromString(string keyValueStr, string keyName)
        {
            string[] strSplit = keyValueStr.Split('&');
            string valueStr = "";

            foreach (string str in strSplit)
            {
                string[] s = str.Split('=');
                string key = s[0];
                string value = s[1];
                if (key == keyName)
                {
                    valueStr = value;
                    break;
                }
            }

            return valueStr;
        }
    }
}
