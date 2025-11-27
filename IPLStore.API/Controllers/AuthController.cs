using IPLStore.API.Models;
using IPLStore.API.Services;
using IPLStore.Application.Interfaces;
using IPLStore.API.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IPLStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private IAuthService _authService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenService jwtTokenService,
            IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _authService = authService;
        }

        // -------------------------
        // POST: api/auth/register
        // -------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid register request");

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return BadRequest("User already exists");

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Registration successful" });
        }

        // -------------------------
        // POST: api/auth/login
        // -------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto, ApplicationUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid login request");

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized("Invalid email or password");

            var token = _jwtTokenService.CreateToken(user);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email
            };

            return Ok(response);
        }

        // -------------------------
        // GET: api/auth/me
        // -------------------------
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                Email = User.Identity.Name
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ResetPasswordDto dto)
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(dto.Email);

            return Ok(new
            {
                message = "If an account exists, reset instructions were sent.",
                token = token // Temporary until email system is added
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PerformResetPasswordDto dto)
        {
            var success = await _authService.ResetPasswordAsync(
                dto.Email, dto.Token, dto.NewPassword
            );

            if (!success)
                return BadRequest("Invalid reset attempt");

            return Ok(new { message = "Password reset successful" });
        }
    }
}
