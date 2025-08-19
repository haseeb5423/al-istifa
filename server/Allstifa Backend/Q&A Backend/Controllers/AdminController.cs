using Microsoft.AspNetCore.Mvc;
using Q_A_Backend.Repositories.Interfaces;

namespace Q_A_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForAdmin(
    [FromQuery] string? search,
    [FromQuery] string? sortBy,
    [FromQuery] string? sortDir,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                var users = await _adminRepository.GetUsersForAdminAsync(search, sortBy, sortDir, pageNumber, pageSize);
                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _adminRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
