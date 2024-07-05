
using Store.Authentication;
using Store.web.App;
using System.Security.Cryptography;
using System.Text;

namespace Store.Data.EF
{
    public class LoginRepository : ILoginRepository
    {
        //UserContext context;
        //public UserRepository(UserContext userContext)
        //{
        //    context = userContext;
        //}
        public int AddUser(string name, string email, string phone, string password)
        {
        //    if (context.Users.Any(x => x.Name == user.Name))
        //        throw new Exception("User is already exist!");
        //    if (user.Role == RoleId.Admin)
        //        if (context.Users.Any(x => x.RoleId == RoleId.Admin))
        //            throw new Exception("Admin is already exist!");

        //    var entity = new User { Name = user.Name, RoleId = user.Role };
        //    entity.Salt = new byte[16];
        //    new Random().NextBytes(entity.Salt);
        //    var data = Encoding.UTF8.GetBytes(user.Password).Concat(entity.Salt).ToArray();
        //    entity.Password = new SHA512Managed().ComputeHash(data);
        //    context.Users.Add(entity);
        //    context.SaveChanges();

            return 0;
        }

        public LoginResponse CheckUser(string login, string pass)
        {
            //var user = context.Users.FirstOrDefault(x => x.Name == login.Name);
            //if (user == null) throw new Exception("No user like this!");
            //var data = Encoding.UTF8.GetBytes(login.Password).Concat(user.Salt).ToArray();
            //var hash = new SHA512Managed().ComputeHash(data);

            //if (user.Password.SequenceEqual(hash))
            //    return (RoleIdDto)user.RoleId;
            //throw new Exception("Wrong password!");
            if (login == "admin" && pass == "admin")
            {
                return new LoginResponse() { Name = login, Result = true, Role = RoleId.Admin };
            }
            else return new LoginResponse() { Name = login, Result = false };
        }
    }
}
