namespace DotNetApi.Models
{
    partial class UserForRegistrationDto
    {
        string Email { get; set; }

        string Password { get; set; }

        string PasswordConfrim { get; set; }

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