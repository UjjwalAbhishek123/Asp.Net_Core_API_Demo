using LearnApiDemo.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace LearnApiDemo.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly LearnApiDbContext _dbContext;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, LearnApiDbContext dbContext) : base(options, logger, encoder)
        {
            _dbContext = dbContext;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //check if we have authorization header
            //if we dont have authorization header, Authentication fails
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No header found");
            }

            var headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]!);

            if (headerValue != null)
            {
                var bytes = Convert.FromBase64String(headerValue.Parameter!);

                string credentials = Encoding.UTF8.GetString(bytes);

                string[] array = credentials.Split(":");

                string username = array[0];

                string password = array[1];

                var user = await _dbContext.TblUsers.FirstOrDefaultAsync(item => item.Username == username && item.Password == password);

                if (user != null)
                {
                    var claim = new[] { new Claim(ClaimTypes.Name, user.Username) };
                    var identity = new ClaimsIdentity(claim, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("UnAuthorized");
                }
            }
            else
            {
                return AuthenticateResult.Fail("Empty Header");
            }
        }
    }
}
