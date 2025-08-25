using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Q_A_Backend.DTOs;
using Q_A_Backend.Repositories.Interfaces;
using Q_A_Backend.Services.Interfaces;

namespace Q_A_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register/step1")]
        public async Task<IActionResult> RegisterStep1Async(RegisterStep1Dto registerStep1Dto)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var allowedRoles = new[] { "Learner", "Scholar" };
            if (!allowedRoles.Contains(registerStep1Dto.Role))
                return BadRequest("Invalid role selected.");

            if (registerStep1Dto == null)
            {
                return BadRequest("Plz Enter appropriate data.");
            }
            var result = await _authService.RegisterStep1Async(registerStep1Dto);
            if (result == null)
            {
                return BadRequest("Registration failed. Please try again.");
            }
            else
            {
                return Ok(new { UserId = result, Message = "Registration step 1 successful." });
            }
            }
            catch (Exception ex)
            {
                return Conflict("User already exists. Please try with a different email");
            }
        }
        [HttpPost("register/step2")]
        public async Task<IActionResult> RegisterStep2Async([FromForm] RegisterStep2Dto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var success = await _authService.RegisterStep2Async(dto);
            if (success)
                return Ok(new { message = "Step 2 completed!" });
            else
                return BadRequest(new { message = "Step 2 failed!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var loginResult = await _authService.LoginAsync(dto);
            if (loginResult.Success)
            {
                return Ok(new { Token = loginResult.Token, user = loginResult.User, Message = "Login successful." });
            }
            else if (loginResult.Success == false && loginResult.Message == "User not found.")
            {
                return NotFound(loginResult.Message);
            }
            else if (loginResult.Success == false && loginResult.Message == "Invalid password.")
            {
                return Unauthorized(loginResult.Message);
            }
            else
            {
                return loginResult.Success == false && loginResult.Message == "Token generation failed."
                    ? StatusCode(StatusCodes.Status500InternalServerError, loginResult.Message)
                    : BadRequest(loginResult.Message);
            }
        }
    }

}

