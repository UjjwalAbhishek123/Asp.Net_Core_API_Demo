using LearnApiDemo.Repositories;
using LearnApiDemo.Services;

namespace LearnApiDemo.ServiceImplementation
{
    public class RefreshHandlerService : IRefreshHandlerService
    {
        private readonly IRefreshHandlerRepository _refreshHandlerRepository;

        public RefreshHandlerService(IRefreshHandlerRepository refreshHandlerRepository)
        {
            _refreshHandlerRepository = refreshHandlerRepository;
        }
        public async Task<string> GenerateToken(string username)
        {
            return await _refreshHandlerRepository.GenerateToken(username);
        }
    }
}
