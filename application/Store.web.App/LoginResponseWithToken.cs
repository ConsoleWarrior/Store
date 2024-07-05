

using Store.Authentication;

namespace Store.web.App
{
    public class LoginResponseWithToken
    {
        public string Name { get; set;}
        public RoleId Role {  get; set;}
        public bool Result { get; set;}
        public string Token { get; set;}
    }
}
