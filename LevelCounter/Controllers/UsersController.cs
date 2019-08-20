using LevelCounter.Models.DTO;
using LevelCounter.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace LevelCounter.Exceptions
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService accountService;

        public UsersController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        // POST api/values
        [HttpPost("create")]
        public async Task<HttpStatusCode> SignUp(SignupRequest signUpRequest)
        {
            await accountService.SignUpAsync(signUpRequest);
            return HttpStatusCode.Created;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
