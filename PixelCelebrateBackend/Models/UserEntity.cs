namespace PixelCelebrateBackend.Models
{
    //No controller:

    public class UserEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        
        //public DateOnly Birthday { get; set; }
        public DateTime Birthday { get; set; }
        public string Role { get; set; } = null!;
        public string Email { get; set; } = null!;
        
        //public DateOnly DateOfJoining { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
