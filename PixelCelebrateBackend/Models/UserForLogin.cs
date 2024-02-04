namespace PixelCelebrateBackend.Models
{
    public class UserForLogin
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        //Constructor 1:
        public UserForLogin()
        {

        }

        //Constructor 2:
        public UserForLogin(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}
