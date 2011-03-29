using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using SuwatAligut2x.Data.Repositories;
using SuwatAligut2x.Data;
using System.Web.Mvc;

namespace SuwatAligut2x.Models
{
    public class UsersModels
    {
        [HiddenInput(DisplayValue=false)]
        [DisplayName("User Id")]
        public int UserId { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Email (Used for Gravatar)")]
        public string Gravatar { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [HiddenInput(DisplayValue=false)]
        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }

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

        public UsersModels CreateNewUser(string openId, string openIdFriendly, string email)
        {
            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                TagIya newUser = tir.CreateNewUser(openId, openIdFriendly, email);
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

        public void UpdateUser()
        {
            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                tir.UpdateUser(this.UserId, this.DisplayName, this.Gravatar, this.FullName, this.Location, this.BirthDate);
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
            if (!string.IsNullOrEmpty(tagIya.Gravatar) && tagIya.Gravatar.Contains("@no-email.com"))
                user.Gravatar = "";
            else
                user.Gravatar = tagIya.Gravatar;
            if (tagIya.BirthDate.HasValue)
                user.Age = (DateTime.Now.Year - tagIya.BirthDate.Value.Year);
            user.Location = tagIya.Location;
            user.BirthDate = tagIya.BirthDate.HasValue ? tagIya.BirthDate.Value : new DateTime();

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
