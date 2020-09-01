using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace _3_25_HW_MVC_Blog.Models
{

    public class BlogDB
    {
        private string _conn;

        public BlogDB(string connection)
        {
            _conn = connection;
        }

        public bool GetPrevLaterPosts(string scope, int @blogId)
        {
            return GetPosts(scope, @blogId).Count > 0;
        }


        public List<Blog> GetPosts(string condition, int blogId)
        {
            List<Blog> Posts = new List<Blog>();

            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"select top 5 BlogId,left(ltrim(rtrim(BlogText)),200) + '...'as BlogText, DateAdded
                                    from BlogPost ";

                //the if sets the order by and the blogId condition >  or <
                if (condition == "Older") //if we are getting previous records, we want to get in desc order, most recent next 5
                {
                    cmd.CommandText += "where blogId < @id  order by DateAdded desc";
                }
                else if (condition == "Newer") //if we are getting newer, we want to get the next 5 in asc
                { 
              
                    cmd.CommandText += "where blogId > @id ";
                }
                else //if it's the first run - no condition is set yet, get in desc 
                {
                    cmd.CommandText += "order by DateAdded Desc";
                }
                
                cmd.Parameters.AddWithValue("@id", blogId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                       
                while (reader.Read())
                {
                    Posts.Add(new Blog
                    {
                        BlogId = (int)reader["BlogId"],
                        BlogText = (string)reader["BlogText"],
                        DateAdded = (DateTime)reader["DateAdded"]
                    });
                }
              
                return Posts;
            }
        }

        public void AddPost(Blog b)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO BLOGPOST (BlogText,DateAdded) 
                                  SELECT @text, GetDate()";
                cmd.Parameters.AddWithValue("@text", b.BlogText);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Blog GetSinglePost(int blogId)
        {
            Blog blog = new Blog();

            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from BlogPost where blogId = @id";
                cmd.Parameters.AddWithValue("@id", blogId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    blog.BlogId = (int)reader["BlogId"];
                    blog.BlogText = (string)reader["BlogText"];
                    blog.DateAdded = (DateTime)reader["DateAdded"];
                }

                return blog;
            }
        }

        public List<BlogComment> GetComments(int blogId)
        {
            List<BlogComment> comments = new List<BlogComment>();

            using(SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select * from BlogComment c where blogId = @id";
                cmd.Parameters.AddWithValue("@id", blogId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comments.Add(new BlogComment
                    {
                        BlogId = (int)reader["BlogId"],
                        CommentId = (int)reader["CommentId"],
                        CommentDate = (DateTime)reader["CommentDate"],
                        CommentAuthor= (string)reader["CommentAuthor"],
                        CommentText=(string)reader["CommentText"]
                    });
                }                
            }

            return comments;
        }

        public void AddComment(BlogComment c)
        {
            using (SqlConnection conn = new SqlConnection(_conn))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO BlogComment (BlogId,CommentText,CommentDate, CommentAuthor)
                                   select @blogId, @text, @date,@author";
                cmd.Parameters.AddWithValue("@blogId", c.BlogId);
                cmd.Parameters.AddWithValue("@text", c.CommentText);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.Parameters.AddWithValue("@author", c.CommentAuthor);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }

}