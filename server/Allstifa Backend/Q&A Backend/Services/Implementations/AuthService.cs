using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Q_A_Backend.DTOs;
using Q_A_Backend.Models;
using Q_A_Backend.Repositories.Interfaces;
using Q_A_Backend.Services.Interfaces;
using QnABackend.Services.Interfaces;

namespace Q_A_Backend.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordService _passwordSvc;
        private readonly IAuthRepository _authRepository;
        private readonly IFileService _fileService;
        private readonly IJwtService _jwtService;

        public AuthService(IPasswordService passwordSvc, IAuthRepository authRepository, IFileService fileService, IJwtService jwtService)
        {
            _passwordSvc = passwordSvc;
            _authRepository = authRepository;
            _fileService = fileService;
            _jwtService = jwtService;
        }



        public async Task<Guid?> RegisterStep1Async(RegisterStep1Dto registerStep1Dto)
        {
            var userExists =  await _authRepository.GetUserByEmailAsync(registerStep1Dto.Email);
            if (userExists != null)
            {
                throw new Exception("User with this email already exists.");
            }
            var step1 = new User
            {
                Name = registerStep1Dto.Name,
                Email = registerStep1Dto.Email,
                PasswordHash = _passwordSvc.HashPassword(registerStep1Dto.Password),
                Role = registerStep1Dto.Role,
                MaslakId = registerStep1Dto.MaslakId
            };
            return await _authRepository.RegisterStep1Async(step1);
        }

        public async Task<bool> RegisterStep2Async(RegisterStep2Dto dto)
        {
            var user = await _authRepository.GetUserByIdAsync(dto.UserId);
            if (user == null)
                return false;

            string? imagePath = null;
            string? proofPath = null;

            if (dto.ProfileImage != null)
                imagePath = await _fileService.SaveFileAsync(dto.ProfileImage, "ProfileImages");

            if (dto.ProofFile != null)
                proofPath = await _fileService.SaveFileAsync(dto.ProofFile, "ProofFiles");

            user.Bio = dto.Bio;
            user.ProfileImagePath = imagePath;
            user.ProofFilePath = proofPath;
            user.SocialMediaLink = dto.SocialMediaLink; // New: set social media link

            return await _authRepository.RegisterStep2Async(user);
        }

        public async Task<LoginResult> LoginAsync(LoginDto dto)
        {
            var user = await _authRepository.LoginAsync(dto);
            if (user == null)
            {
                return new LoginResult { Success = false, Message = "User not found." };
            }
            if (!_passwordSvc.VerifyPassword(user.PasswordHash, dto.Password))
            {
                return new LoginResult { Success = false, Message = "Invalid password." };
            }
            var token = _jwtService.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                return new LoginResult { Success = false, Message = "Token generation failed." };
            }

            return new LoginResult { Success = true, Token = token };
        }
    }
}
