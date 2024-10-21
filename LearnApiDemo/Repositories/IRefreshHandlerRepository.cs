namespace LearnApiDemo.Repositories
{
    public interface IRefreshHandlerRepository
    {
        Task<string> GenerateToken(string username);
    }
}
