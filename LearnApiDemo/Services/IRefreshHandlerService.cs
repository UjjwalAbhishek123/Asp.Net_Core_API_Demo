namespace LearnApiDemo.Services
{
    public interface IRefreshHandlerService
    {
        Task<string> GenerateToken(string username);
    }
}
