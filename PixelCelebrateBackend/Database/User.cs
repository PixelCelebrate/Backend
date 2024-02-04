using System;
using System.Collections.Generic;

namespace PixelCelebrateBackend.Database
{
    public partial class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfJoining { get; set; }

        //Constructor 1:
        public User()
        {

        }

        //Constructor 2:
        public User(int UserId, string Firstname, string LastName, 
                    string UserName, string Password, DateTime Birthday, 
                    string Role, string Email, DateTime DateOfJoining)
        {
            this.UserId = UserId;
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
