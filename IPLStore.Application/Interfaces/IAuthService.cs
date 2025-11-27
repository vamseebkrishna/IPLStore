namespace IPLStore.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    }
}
