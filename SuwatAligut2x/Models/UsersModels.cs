using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using SuwatAligut2x.Data.Repositories;
using SuwatAligut2x.Data;

namespace SuwatAligut2x.Models
{
    public class UsersModels
    {
        [DisplayName("User Id")]
        public int UserId { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("User Avatar")]
        public string Gravatar { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Users OpenId's")]
        public List<OpenIdsModels> OpenId { get; set; }

        public UsersModels GetUserByOpenId(string openId)
        {
            if (string.IsNullOrEmpty(openId))
                throw new ArgumentNullException("OpenId");

            TagIyaRepository tir = new TagIyaRepository();
            TagIya tagIya = tir.GetTagIyaByOpenId(openId);
            if (tagIya != null)
            {
                return GetUserModel(tagIya);
            }
            else
            {
                return null;
            }
        }

        public void CreateOpenIdForUser(string openId, string openIdFriendly)
        {
            if (UserId < 0)
                throw new ArgumentNullException("UserId", "User needed for OpenId.");

            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                tir.CreateOpenIdForUser(this.UserId, openId, openIdFriendly);
            }
            catch
            {
                throw;
            }
        }

        public UsersModels CreateNewUser(string openId, string openIdFriendly)
        {
            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                TagIya newUser = tir.CreateNewUser(openId, openIdFriendly);
                return GetUserModel(newUser);
            }
            catch
            {
                throw;
            }
        }

        public UsersModels GetUser()
        {
            if (this.UserId < 0)
                throw new ArgumentNullException("UserId", "Primary key missing");

            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                var tagIya = tir.GetTagIya(this.UserId);
                return GetUserModel(tagIya);
            }
            catch
            {
                throw;
            }
        }

        private UsersModels GetUserModel(TagIya tagIya)
        {
            UsersModels user = new UsersModels();
            user.OpenId = OpenIdsModels.ParseOpenIds(tagIya.OpenIdSaTagIyas.ToList());
            user.UserId = tagIya.UserId;
            user.DisplayName = tagIya.ScreenName;
            user.FullName = tagIya.RealName;
            user.Gravatar = tagIya.Gravatar;
            if (tagIya.BirthDate.HasValue)
                user.Age = (DateTime.Now.Year - tagIya.BirthDate.Value.Year);
            user.Location = tagIya.Location;

            return user;
        }
    }

    #region Service
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion
}
