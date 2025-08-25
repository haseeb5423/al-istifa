using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Q_A_Backend.DTOs;
using Q_A_Backend.Repositories.Interfaces;

namespace Q_A_Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        [HttpGet("{id}")]
        // [Authorize]
        public async Task<IActionResult> GetProfileByIdAsync(Guid id)
        {
            try
            {
                var profile = await _profileRepository.GetProfileByIdAsync(id);
                if (profile == null)
                {
                    return NotFound("User not found");
                }
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while fetching profile: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        // [Authorize]
        public async Task<IActionResult> UpdateProfileAsync(Guid id, [FromBody] ProfileUpdateDto profileUpdateDto)
        {
            if (profileUpdateDto == null || id == Guid.Empty)
            {
                return BadRequest("Invalid profile data.");
            }

            try
            {
                var success = await _profileRepository.UpdateProfileAsync(id, profileUpdateDto);
                if (success)
                {
                    return Ok("Profile updated successfully.");
                }
                else
                {
                    return NotFound("Profile not found or update failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while updating profile: {ex.Message}");
            }
        }


    }
}