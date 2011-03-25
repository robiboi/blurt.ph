using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuwatAligut2x.Data;

namespace SuwatAligut2x.Models
{
    public class OpenIdsModels
    {
        public string OpenId { get; set; }
        public string FriendlyOpenId { get; set; }
        public DateTime DateCreated { get; set; }

        public static List<OpenIdsModels> ParseOpenIds(List<OpenIdSaTagIya> OpenIds)
        {
            List<OpenIdsModels> oiList = new List<OpenIdsModels>();
            foreach (OpenIdSaTagIya oiti in OpenIds)
            {
                OpenIdsModels oi = new OpenIdsModels();
                oi.OpenId = oiti.OpenId;
                oi.FriendlyOpenId = oiti.FriendlyOpenId;
                oi.DateCreated = oiti.DateCreated;
                oiList.Add(oi);
            }

            return oiList;
        }
    }
}
