namespace DotNetApi.Models
{
    public partial class UserForRegistrationDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfrim { get; set; }

        public UserForRegistrationDto()
        {
            if( Email == null)
            {
                Email = "";
            }
            if( Password == null)
            {
                Password = "";
            }
            if( PasswordConfrim == null)
            {
                PasswordConfrim = "";
            }
        }

    }
}