using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Q_A_Backend.DTOs;
using Q_A_Backend.Repositories.Interfaces;
using QnABackend.Services.Interfaces;

namespace Q_A_Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IFileService _fileService;

        public ProfileController(IProfileRepository profileRepository, IFileService fileService)
        {
            _profileRepository = profileRepository;
            _fileService = fileService;
        }
        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
    public async Task<IActionResult> UpdateProfileAsync(Guid id, [FromForm] ProfileUpdateDto profileUpdateDto, [FromForm] IFormFile? profileImage, [FromForm] IFormFile? proofFile)
        {
            if (profileUpdateDto == null || id == Guid.Empty)
            {
                return BadRequest("Invalid profile data.");
            }

            try
            {
                // If image files were uploaded, save them and populate the DTO with the returned URLs
                if (profileImage != null)
                {
                    var imageUrl = await _fileService.SaveFileAsync(profileImage, "profiles");
                    profileUpdateDto.ProfileImagePath = imageUrl;
                }

                if (proofFile != null)
                {
                    var proofUrl = await _fileService.SaveFileAsync(proofFile, "proofs");
                    profileUpdateDto.ProofFilePath = proofUrl;
                }

                var updatedUser = await _profileRepository.UpdateProfileAsync(id, profileUpdateDto);
                if (updatedUser == null)
                {
                     return StatusCode(500, "Internal server error while updating profile.");
                }
                else
                {
                return new OkObjectResult(updatedUser);
                }
                   
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while updating profile: {ex.Message}");
            }
        }


    }
}