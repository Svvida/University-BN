using Application.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetTokenStore _passwordResetTokenStore;
        private readonly ILogger<PasswordResetController> _logger;

        public PasswordResetController(IPasswordResetService passwordResetService,
            IEmailService emailService,
            IPasswordResetTokenStore passwordResetTokenStore,
            ILogger<PasswordResetController> logger)
        {
            _passwordResetService = passwordResetService;
            _emailService = emailService;
            _passwordResetTokenStore = passwordResetTokenStore;
            _logger = logger;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // Generate reset identifier
            var resetIdentifier = HttpContext.Session.Id;

            // Generate reset token
            var resetToken = _passwordResetTokenStore.GenerateToken(dto.Email, resetIdentifier);

            // Store reset identifier in a secure cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(30)
            };
            Response.Cookies.Append("PasswordResetIdentifier", resetIdentifier, cookieOptions);

            // Construct reset link
            var resetLink = Url.Action("ResetPassword", "PasswordReset", new { token = resetToken, email = dto.Email }, Request.Scheme);

            // Send email with reset link
            await _emailService.SendEmailAsync(dto.Email, "Password Reset Request", $"Click the link to reset your password: {resetLink}");

            return Ok(new { message = "If an account with that email exists, a password reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] string email, [FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve reset identifier from secure cookie
            var resetIdentifier = Request.Cookies["PasswordResetIdentifier"];

            if (!_passwordResetTokenStore.ValidateToken(token, email, resetIdentifier))
            {
                return BadRequest(new { message = "Invalid or expired token, or the request was made from a different browser or device." });
            }

            if (await _passwordResetService.CheckLastPasswordAsync(email, dto.Password))
            {
                return BadRequest(new { message = "New password cannot be the same as the current password." });
            }

            await _passwordResetService.ResetUserPassword(email, dto.Password);
            _passwordResetTokenStore.InvalidateToken(token);

            return Ok(new { message = "Password has been reset successfully" });
        }
    }
}
