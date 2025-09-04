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
[Consumes("multipart/form-data")]
[Authorize]
public async Task<IActionResult> UpdateProfileAsync(
    Guid id,
    [FromForm] ProfileUpdateDto profileUpdateDto) // Only one [FromForm] parameter
{
    if (profileUpdateDto == null || id == Guid.Empty)
    {
        return BadRequest("Invalid profile data.");
    }

    try
    {
        // Handle profile image upload from the DTO property
        if (profileUpdateDto.ProfileImage != null)
        {
            var imageUrl = await _fileService.SaveFileAsync(profileUpdateDto.ProfileImage, "profiles");
            profileUpdateDto.ProfileImagePath = imageUrl;
        }

        // Handle proof file upload from the DTO property
        if (profileUpdateDto.ProofFile != null)
        {
            var proofUrl = await _fileService.SaveFileAsync(profileUpdateDto.ProofFile, "proofs");
            profileUpdateDto.ProofFilePath = proofUrl;
        }

        var updatedUser = await _profileRepository.UpdateProfileAsync(id, profileUpdateDto);

        if (updatedUser == null)
        {
            return Problem(
                detail: "Profile could not be updated. Repository returned null.",
                statusCode: 500,
                title: "Internal server error"
            );
        }

        return Ok(updatedUser);
    }
    catch (Exception ex)
    {
        return Problem(
            detail: ex.Message,
            statusCode: 500,
            title: "Internal server error while updating profile"
        );
    }
}


    }
}