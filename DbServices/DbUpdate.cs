using DbAdapter;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace DbServices
{
    public static class DbUpdate
    {
        static AppDbContext db = new AppDbContext();

        public static void CreateUser(string userName, string password)
        {
            var user = new User { UserName = userName, Password = password };
            db.Add(user);
            db.SaveChanges();
        }

        public static bool CheckUser(string userName, string password)
        {
            var user = db.User.Where(u => u.UserName == userName && u.Password == password).ToArray();
            if (user.Length == 0)
                return false;
            else
                return true;
        }

        public static void CreatePost(string text, int userId, string[] types)
        {
            var post = new Post() { Text = text, User = new User { Id = userId }, postCategory = new List<PostCategory>() };
            foreach (var type in types)
            {
                var ct = db.Category.First(c => c.Type.ToString() == type);
                post.postCategory.Add(new PostCategory { Post = post, Category = ct });
            }

            db.Add(post);
            db.SaveChanges();
        }

        public static int GetUserId(string userName)
        {
            var userId = db.User.Where(u => u.UserName == userName).Select(u => u.Id).Single();

            return userId;
        }

        public static Post[] GetLatestPosts()
        {

            return db.Post.Include(p => p.postCategory).ThenInclude(pc => pc.Category).Include(p => p.User).OrderByDescending(p => p.Date).ToArray();

        }

        public static void LikePost(int postId)
        {
            var post = db.Post.Single(p => p.Id == postId);
            post.Like += 1;
            db.SaveChanges();
        }

        public static Post[] GetMostPopular()
        {

            return db.Post.Include(p => p.User).Include(p => p.postCategory).ThenInclude(pc => pc.Category).OrderByDescending(p => p.Like).Take(5).ToArray();

        }

        public static string[] GetAllCatergories()
        {
            var categories = db.Category.Select(c => c.Type).ToArray();
            var types = new string[categories.Length];

            for (int i = 0; i < types.Length; i++)
            {
                types[i] = categories[i].ToString();
            }

            return types;

        }


    }


}
