namespace PixelCelebrateBackend.Models
{
    public class UserEmployeeTable
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;

        //Constructor 1:
        public UserEmployeeTable()
        {

        }

        //Constructor 2:
        public UserEmployeeTable(string UserName, string Email)
        {
            this.UserName = UserName;
            this.Email = Email;
        }
    }
}
