using Store.Authentication;

namespace Store
{
    public interface ILoginRepository
    {
        int AddUser(string name, string email, string phone, string password);
        LoginResponse CheckUser(string login, string pass);
    }
}
