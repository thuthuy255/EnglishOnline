using Model.DTO;

namespace API.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Login(LoginDto model);
        Task<ApiResponse<object>> Register(RegisterDTO model);
        Task<ApiResponse<object>> CheckEmail(string email);
        Task<ApiResponse<object>> VerifyOTP(string otp,string email);

        Task<ApiResponse<object>> ForgotPassword(ForgotPasswordDTO model);
    }
}
