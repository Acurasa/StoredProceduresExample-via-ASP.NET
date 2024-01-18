namespace StoredProcuduresTest.Dtos
{
    public partial class UserLoginConfirmation
    {
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; } = new byte[0];

    }
}
