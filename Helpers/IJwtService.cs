namespace JwtAuthentication.Helpers
{
    public interface IJwtService
    {
        string GenerateToken(int id);
    }
}