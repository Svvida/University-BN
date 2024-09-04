using Application.DTOs.Account.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAsync();

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID format");
            }

            var account = await _accountService.GetByIdAsync(id);
            if (account is null)
            {
                return NotFound($"Account with ID: {id} was not found");
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDto accountCreateDto)
        {
            if (accountCreateDto is null)
            {
                return BadRequest("Account data must be provided");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _accountService.CreateAsync(accountCreateDto);
                return CreatedAtAction(nameof(GetAccountById), new { id = accountCreateDto.Id }, accountCreateDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the account");
            }
        }

        [HttpPost("updatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            _logger.LogInformation($"Received AccountId: {dto.AccountId}");
            if (string.IsNullOrWhiteSpace(dto.NewPassword) || dto.NewPassword.Length < 8)
            {
                return BadRequest("Password must be at lease 8 characters long");
            }

            try
            {
                await _accountService.UpdatePasswordAsync(dto.AccountId, dto.NewPassword);
                return Ok("Password updated successfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Account not found");
            }
        }
    }
}
