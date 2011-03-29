using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuwatAligut2x.Models
{
    public class RegisterOpenId
    {
        public string ClaimedOpenId { get; set; }
        public string FriendlyOpenId { get; set; }
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
    }
}
