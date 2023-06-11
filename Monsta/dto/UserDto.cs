namespace Monsta.dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = String.Empty;

        public string UserEmail { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
