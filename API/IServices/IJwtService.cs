namespace API.IServices
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, string role);
    }

}
