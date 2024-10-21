using LearnApiDemo.Data;
using LearnApiDemo.DTOs;
using LearnApiDemo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly LearnApiDbContext _dbContext;

        private readonly JwtSettings _jwtSettings;

        private readonly IRefreshHandlerService _refreshHandlerService;

        public AuthorizeController(LearnApiDbContext dbContext, IOptions<JwtSettings> options, IRefreshHandlerService refreshHandlerService)
        {
            _dbContext = dbContext;
            _jwtSettings = options.Value; //after this we'll be able to access our security key
            _refreshHandlerService = refreshHandlerService;
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] UserCredDto userCred) 
        {
            var user = await _dbContext.TblUsers.FirstOrDefaultAsync(item => item.Username == userCred.username && item.Password == userCred.password);

            if (user != null)
            {
                //if user is not null => generate token

                //initiate tokenHandler
                var tokenHandler = new JwtSecurityTokenHandler();

                //convert security key into byte
                var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);

                //initiate token descriptor
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires=DateTime.UtcNow.AddSeconds(30),
                    SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(tokenKey),SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                var finalToken = tokenHandler.WriteToken(token);

                return Ok(new TokenResponse() { Token = finalToken, RefreshToken = await _refreshHandlerService.GenerateToken(userCred.username) });
            }
            else
            {
                //Unauthorized
                return Unauthorized();
            }
        }

        //Generate RefreshToken based on expired JWT Token
        [HttpPost("GenerateRefreshToken")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenResponse token)
        {
            //in method parameter we passed both JWT token and RefreshToken
            var _refreshToken = await _dbContext.TblRefreshTokens.FirstOrDefaultAsync(item => item.RefreshToken == token.RefreshToken);

            if (_refreshToken != null)
            {
                //if _refreshToken is not null => generate token

                //initiate tokenHandler
                var tokenHandler = new JwtSecurityTokenHandler();

                //convert security key into byte
                var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.securityKey);

                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token.Token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out securityToken);

                var _token = securityToken as JwtSecurityToken;

                if(_token!=null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    string? username = principal.Identity?.Name;

                    var _existingData = await _dbContext.TblRefreshTokens.FirstOrDefaultAsync(item => item.UserId == username && item.RefreshToken == token.RefreshToken);

                    if(_existingData != null)
                    {
                        var newToken = new JwtSecurityToken(
                                claims: principal.Claims.ToArray(),
                                expires: DateTime.Now.AddSeconds(30),
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.securityKey)),
                            SecurityAlgorithms.HmacSha256)
                        );

                        var _finalToken = tokenHandler.WriteToken(newToken);

                        return Ok(new TokenResponse() { Token = _finalToken, RefreshToken = await _refreshHandlerService.GenerateToken(username) });
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    //Unauthorized
                    return Unauthorized();
                }

                ////initiate token descriptor
                //var tokenDescriptor = new SecurityTokenDescriptor
                //{
                //    Subject = new ClaimsIdentity(new Claim[]
                //    {
                //        new Claim(ClaimTypes.Name, user.Username),
                //        new Claim(ClaimTypes.Role, user.Role)
                //    }),
                //    Expires = DateTime.UtcNow.AddSeconds(30),
                //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                //};

                //var token = tokenHandler.CreateToken(tokenDescriptor);

                //var finalToken = tokenHandler.WriteToken(token);

                //return Ok(new TokenResponse() { Token = finalToken, RefreshToken = await _refreshHandlerService.GenerateToken(userCred.username) });
            }
            else
            {
                //Unauthorized
                return Unauthorized();
            }
        }
    }
}
