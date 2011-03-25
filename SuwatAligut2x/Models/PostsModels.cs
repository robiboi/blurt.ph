using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.ComponentModel;
using SuwatAligut2x.Data;
using SuwatAligut2x.Data.Repositories;

namespace SuwatAligut2x.Models
{
    public class PostsModels
    {
        [DisplayName("Display Name")]
        /// <summary>
        /// Display Name or Screen name
        /// </summary>
        public string DisplayName { get; set; }

        [DisplayName("User Id"), Description("Used when posting a message.")]
        /// <summary>
        /// User Id of the poster
        /// </summary>
        public int PostUserId { get; set; }

        [DisplayName("Gravatar"), Description("Email used for Gravatar.")]
        /// <summary>
        /// Email used for Gravatar
        /// </summary>
        public string Gravatar { get; set; }
        
        [DisplayName("Message")]
        /// <summary>
        /// Post Message
        /// </summary>
        public string Message { get; set; }

        [DisplayName("Message identifier")]
        /// <summary>
        /// Message identifier
        /// </summary>
        public int MessageId { get; set; }

        [DisplayName("Message Comments")]
        /// <summary>
        /// Message Comments
        /// </summary>
        public List<CommentsModels> Comments { get; set; }

        [DisplayName("Date Posted")]
        /// <summary>
        /// Date of the Message posted
        /// </summary>
        public DateTime DatePosted { get; set; }

        [DisplayName("Number of Votes")]
        /// <summary>
        /// Number of Votes of the Message
        /// </summary>
        public int NumberOfVotes { get; set; }

        private PostsModels GetPostsModel(TagIya tagIya, Mensahe msg)
        {
            MensaheRepository mr = new MensaheRepository();
            PostsModels post = new PostsModels();
            post.DisplayName = tagIya.ScreenName;
            post.Message = msg.Message;
            post.Comments = CommentsModels.ParseComments(msg.Kumentaryos.ToList());
            post.DatePosted = msg.DatePosted;
            post.NumberOfVotes = mr.GetMessageVotes(msg.MessageId);
            post.PostUserId = tagIya.UserId;
            post.MessageId = msg.MessageId;
            if (string.IsNullOrEmpty(tagIya.Gravatar))
                post.Gravatar = Utility.GetMD5Hash("noemail@robiboi.com");
            else
                post.Gravatar = Utility.GetMD5Hash(tagIya.Gravatar);

            return post;
        }

        /// <summary>
        /// Get Posts with paging.
        /// </summary>
        /// <param name="PageSize">Number of results per page.</param>
        /// <param name="Index">Last Index of result</param>
        /// <returns></returns>
        public List<PostsModels> GetPosts(int PageSize, int Index)
        {
            MensaheRepository mr = new MensaheRepository();
            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                var msgs = mr.GetAllMessage().OrderByDescending(m=>m.DatePosted).ToList();

                if (msgs.Count > PageSize)
                    msgs = msgs.GetRange(Index, PageSize);

                List<PostsModels> posts = new List<PostsModels>();
                
                foreach (Mensahe msg in msgs)
                {
                    var tagIya = tir.GetUserbyMsgId(msg.MessageId);
                    PostsModels post = this.GetPostsModel(tagIya, msg);

                    posts.Add(post);
                }
                return posts;
            }
            catch
            {
                throw;
            }
        }
        
        /// <summary>
        /// Get posts by User
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <returns></returns>
        public static List<PostsModels> GetPostsByUser(int UserId)
        {
            MensaheRepository mr = new MensaheRepository();
            TagIyaRepository tir = new TagIyaRepository();

            try
            {
                var msgs = mr.GetUserMessages(UserId);
                var tagIya = tir.GetTagIya(UserId);

                List<PostsModels> posts = new List<PostsModels>();
                foreach (Mensahe msg in msgs)
                {
                    PostsModels post = new PostsModels();
                    post = post.GetPostsModel(tagIya, msg);
                    posts.Add(post);
                }
                return posts;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Post by Message id
        /// </summary>
        /// <param name="MessageId">Message Id</param>
        /// <returns></returns>
        public static PostsModels GetPostByMessageId(int MessageId)
        {
            MensaheRepository mr = new MensaheRepository();
            TagIyaRepository tir = new TagIyaRepository();

            try
            {
                var msg = mr.GetMessage(MessageId);
                var tagIya = tir.GetUserbyMsgId(MessageId);

                PostsModels post = new PostsModels();
                post = post.GetPostsModel(tagIya, msg);

                return post;
            }
            catch
            {
                throw;
            }
        }

        public void CreatePost()
        {
            MensaheRepository mr = new MensaheRepository();
            TagIyaRepository tir = new TagIyaRepository();
            try
            {
                Mensahe msg = new Mensahe();
                msg.Message = this.Message;
                msg.DatePosted = DateTime.Now;
                msg.DateEdited = DateTime.Now;

                mr.InsertMessage(msg, tir.GetTagIya(this.PostUserId));
            }
            catch
            {
                throw;
            }
        }
    }
}
