using Model.DTO;

namespace API.IServices
{
    public interface IUserService
    {
       Task<ApiResponse<object>> GetUserInfo(int idUser);
    }
}
