using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3_25_HW_MVC_Blog.Models
{
    public class BlogViewModel
    {
        public Blog Blog { get; set; }
        public IEnumerable<Blog> BlogList {get; set; }
        public List<BlogComment>Comments { get; set; }

        public string AuthorCookie { get; set; }
        public bool ExistsPrevBlogs { get; set; }
        public bool ExistsNewerBlogs { get; set; }

    }
}
