namespace DotNetApi.Dtos
{
    partial class UserForLoginConfrimationDto
    {
        byte[] PasswordHash { get; set; }

        byte[] PasswordSalt { get; set; }

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