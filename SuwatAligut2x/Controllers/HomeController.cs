using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuwatAligut2x.Models;
using SuwatAligut2x.Helpers;

namespace SuwatAligut2x.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Famous()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }

        public ActionResult Faqs()
        {
            return View();
        }

        /// <summary>
        /// Get Post View result
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="size">size of the page to returl, default = 10</param>
        /// <returns></returns>
        public PartialViewResult Posts(int? page, int? size)
        {
            List<PostsModels> lstPost = new List<PostsModels>();
            try
            {
                lstPost = PostHelper.GetPostPage(page, size);
            }
            catch
            {
                RedirectToAction("Error");
            }
            return PartialView(lstPost);
        }

        public PartialViewResult CreatePost()
        {
            return PartialView();
        }

        [Authorize]
        [HttpPost]
        public PartialViewResult CreatePost(FormCollection form)
        {
            PostsModels post = new PostsModels();
            post.Message = form["NewPost"];
            post.PostUserId = PostHelper.GetUserId(HttpContext.User.Identity.Name);

            post.CreatePost();

            List<PostsModels> lstPost = PostHelper.GetPostPage(0, 10);

            return PartialView("Posts", lstPost);
        }
    }
}
