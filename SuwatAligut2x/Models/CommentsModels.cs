using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using SuwatAligut2x.Data;
using SuwatAligut2x.Data.Repositories;

namespace SuwatAligut2x.Models
{
    public class CommentsModels
    {
        [DisplayName("Comment Id")]
        public int CommentId { get; set; }

        [DisplayName("Message that is being commented")]
        public int MessageId { get; set; }

        [DisplayName("User Id of the Commentor")]
        public int CommentorsId { get; set; }

        [DisplayName("Commentors Display Name")]
        public string CommentorsDisplayName { get; set; }

        [DisplayName("Comment content")]
        public string Comment { get; set; }

        [DisplayName("Date Commented")]
        public DateTime CommentDate { get; set; }

        public void CreateComment()
        {
            KumentaryoRepository kr = new KumentaryoRepository();
            try
            {
                kr.InsertComment(this.Comment, this.MessageId, this.CommentorsId);
            }
            catch
            {
                throw;
            }
        }

        #region Static Methods
        public static List<CommentsModels> ParseComments(List<Kumentaryo> kumentaryo)
        {
            List<CommentsModels> cmList = new List<CommentsModels>();
            foreach (Kumentaryo k in kumentaryo)
            {
                CommentsModels cm = new CommentsModels();
                cm.Comment = k.Comment;
                cm.CommentDate = k.DatePosted;
                cm.CommentId = k.CommentId;
                cm.CommentorsId = k.CommentorsId;
                cm.CommentorsDisplayName = k.TagIya.ScreenName;
                cm.MessageId = k.MessageId;
                cmList.Add(cm);
            }

            return cmList;
        }
        #endregion
    }
}
