using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers.Account
{
    [ApiController]
    [Route("api/")]
    public class VisibilityFieldsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public VisibilityFieldsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // https://localhost:8800/api/userInfo?visibilityFields[]=name&visibilityFields[]=surname&visibilityFields[]=organizer
        [HttpGet("userInfo")]
        public async Task<IActionResult> GetUserInfo([FromQuery] string[] visibilityFields)
        {
            var userClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userClaim) || !Guid.TryParse(userClaim, out var accountId))
            {
                return Unauthorized("Invalid or missing user ID");
            }

            // Fetch full visibility data
            var visibilityData = await _accountService.GetVisibilityFieldsAsync(accountId);

            return Ok(visibilityData);
        }
    }
}
