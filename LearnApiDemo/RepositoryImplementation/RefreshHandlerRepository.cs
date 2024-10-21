using LearnApiDemo.Data;
using LearnApiDemo.Models;
using LearnApiDemo.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LearnApiDemo.RepositoryImplementation
{
    public class RefreshHandlerRepository : IRefreshHandlerRepository
    {
        private readonly LearnApiDbContext _dbContext;

        public RefreshHandlerRepository(LearnApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateToken(string username)
        {
            //here we have to generate refresh token

            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);

                string refreshToken = Convert.ToBase64String(randomNumber);

                var existingToken = _dbContext.TblRefreshTokens.FirstOrDefaultAsync(item => item.UserId == username).Result;

                if (existingToken != null) 
                {
                    existingToken.RefreshToken = refreshToken;
                }
                else
                {
                    //make new entry
                    await _dbContext.TblRefreshTokens.AddAsync(new TblRefreshToken
                    {
                        UserId = username,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken = refreshToken
                    });
                }
                await _dbContext.SaveChangesAsync();

                return refreshToken;
            }
            
        }
    }
}
