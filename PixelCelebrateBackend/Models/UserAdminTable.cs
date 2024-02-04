namespace PixelCelebrateBackend.Models
{
    public class UserAdminTable
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfJoining { get; set; }


        //Constructor 1:
        public UserAdminTable()
        {

        }

        //Constructor 2:
        public UserAdminTable(string Firstname, string LastName,
                    string UserName, string Password, DateTime Birthday,
                    string Role, string Email, DateTime DateOfJoining)
        {
            this.FirstName = Firstname;
            this.LastName = LastName;
            this.UserName = UserName;
            this.Password = Password;
            this.Birthday = Birthday;
            this.Role = Role;
            this.Email = Email;
            this.DateOfJoining = DateOfJoining;
        }
    }
}
