

using Store.Authentication;

namespace Store.web.App
{
    public class LoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public LoginResponse TryLogining(string login, string pass)
        {
            var loginModel = _loginRepository.CheckUser(login, pass);
            return loginModel;
        }

        public bool TryRegistration(string name, string email, string phone, string password)
        {
            if (_loginRepository.AddUser(name, email, phone, password) != 0)
                return true;
            return false;
        }
    }
}
