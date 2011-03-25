using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SuwatAligut2x.Data.Repositories
{
    public class TagIyaRepository : BaseRepository
    {
        public TagIya GetTagIya(int UserId)
        {
            try
            {
                var tagIya = (from t in context.TagIyas
                              where t.UserId == UserId
                              select t).FirstOrDefault();
                return tagIya;
            }
            catch
            {
                throw;
            }
        }

        public TagIya GetTagIyaByOpenId(string OpenId)
        {
            try
            {
                var tagIya = (from t in context.OpenIdSaTagIyas
                              where t.OpenId == OpenId
                              select t.TagIya).FirstOrDefault();

                return tagIya;
            }
            catch
            {
                throw;
            }
        }

        public TagIya GetUserbyMsgId(int MsgId)
        {
            try
            {
                var tagIya = (from mt in context.MensaheSaTagIyas
                              join t in context.TagIyas on mt.UserId equals t.UserId
                              where mt.MessageId == MsgId
                              select t).FirstOrDefault();

                return tagIya;
            }
            catch
            {
                throw;
            }
        }

        public void CreateOpenIdForUser(int UserId, string OpenId, string OpenIdFriendly)
        {
            try
            {
                context.SPI_PunoUgOpenId(UserId, OpenId, OpenIdFriendly);
            }
            catch
            {
                throw;
            }
        }

        public TagIya CreateNewUser(string OpenId, string OpenIdFriendly)
        {
            try
            {
                int userId = context.SPI_BagOngTagIya(OpenId, OpenIdFriendly);

                return GetTagIya(userId);
            }
            catch
            {
                throw;
            }
        }
    }
}
