namespace DotNetApi.Dtos
{
    public partial class UserForLoginConfrimationDto
    {
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        UserForLoginConfrimationDto()
        {
            if(PasswordHash == null)
            {
                PasswordHash = new byte[0];
            }
            if(PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }
    }
}