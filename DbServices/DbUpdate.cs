using DbAdapter;
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

    }


}
