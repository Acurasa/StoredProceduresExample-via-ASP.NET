using StoredProcuduresTest.Dtos;

namespace StoredProcuduresTest.Helpers
{
    public interface IAuthHelper
    {
        public string CreateToken(int userId);
        public byte[] GetPasswordHash(string password, byte[] passwordSalt);

        public Task<bool> SetPasswordAsync(UserForLogin userForSetPassword);
    }
}
