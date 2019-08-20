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

        [HttpPost("create")]
        public async Task<ActionResult> SignUp(SignupRequest signUpRequest)
        {
            var errors = await accountService.SignUpAsync(signUpRequest);
            if (errors.Count == 0)
            {
                return new ObjectResult(errors)
                {
                    StatusCode = 201
                };
            }
            return BadRequest(errors);
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
