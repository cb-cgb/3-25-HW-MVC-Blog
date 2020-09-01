using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _3_25_HW_MVC_Blog.Models;

namespace _3_25_HW_MVC_Blog.Controllers
{
    public class HomeController : Controller
    {
        private string _conn = @"Data Source=.\sqlexpress;Initial Catalog=PeopleDB;Integrated Security=True;";


        public IActionResult Index(string condition,int blogId)
        {
            
            BlogDB db = new BlogDB(_conn);
            BlogViewModel vm = new BlogViewModel();
            vm.BlogList = db.GetPosts(condition, blogId).OrderByDescending(b => b.BlogId);//we want to display the db results in desc sort

            // exclude on the first run. if it's the first run, it will take the top latest 5 so there would not be anything later .
            if (blogId == 0)//first run
            {
                vm.ExistsNewerBlogs = false;
            }
            else
            {
                vm.ExistsNewerBlogs = db.GetPrevLaterPosts("Newer", vm.BlogList.Max(b => b.BlogId));
            }

            vm.ExistsPrevBlogs = db.GetPrevLaterPosts("Older", vm.BlogList.Min(b => b.BlogId));
            return View(vm) ;
        }

        public IActionResult AddPostForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(Blog b)
        {
            BlogDB db = new BlogDB(_conn);
            db.AddPost(b);
            return Redirect( $"/Home/IndexPost?BlogId={b.BlogId}");

        }

        public IActionResult IndexPost( int blogId)
        {
            BlogDB db = new BlogDB(_conn);
            BlogViewModel vm = new BlogViewModel();
            vm.Blog = db.GetSinglePost(blogId);
            vm.Comments = db.GetComments(blogId);
            vm.AuthorCookie = Request.Cookies["authorCookie"];
            return View(vm);
        }
        //public IActionResult AddCommentForm()
        //{
        //    return View();
        //}

        [HttpPost]
        public IActionResult AddComment(BlogComment c)
        {
            Response.Cookies.Append("authorCookie", $"{c.CommentAuthor}");

            BlogDB db = new BlogDB(_conn);
            db.AddComment(c);
            return Redirect($"/Home/IndexPost?blogId={ c.BlogId}");
        }
    }
}
