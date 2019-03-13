using DbAdapter;
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
    }


}
