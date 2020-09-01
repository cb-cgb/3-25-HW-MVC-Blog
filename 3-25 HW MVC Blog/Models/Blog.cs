using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3_25_HW_MVC_Blog.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string BlogText { get; set; }
        public DateTime DateAdded { get; set; }

    }

    public class BlogComment
    {
        public int CommentId { get; set; }
        public int BlogId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public String CommentAuthor { get; set; }
    }
}
