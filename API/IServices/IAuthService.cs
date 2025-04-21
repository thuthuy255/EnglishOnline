using Model.DTO;

namespace API.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Login(LoginDto model);
        Task<ApiResponse<object>> Register(RegisterDTO model);
    }
}
