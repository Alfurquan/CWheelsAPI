using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationPlugin;
using AutoMapper;
using CWheelsAPI.Controllers.Resources;
using CWheelsAPI.Core;
using CWheelsAPI.Errors;
using CWheelsAPI.Filters;
using CWheelsAPI.Models;
using CWheelsAPI.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CWheelsAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IWebHostEnvironment host;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly AuthService _auth;
        private readonly PhotoSettings photoSettings;

        public AccountsController(
            IOptionsSnapshot<PhotoSettings> options, 
            IWebHostEnvironment host,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.host = host;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.mapper = mapper;
            _auth = new AuthService(configuration);
            this.photoSettings = options.Value;
        }


        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <response code="400">For Bad requests</response>
        [HttpPost]
        [ValidationModel]
        public async Task<IActionResult> Register([FromBody]RegisterUserResource registerUserResource)
        {
            try {
                var user = mapper.Map<RegisterUserResource, User>(registerUserResource);

                var userWithSameEmail = unitOfWork.Accounts.FindUserByEmail(user.Email);
                if (userWithSameEmail != null)
                    return BadRequest(new ApiResponse(400, "User with same email already exists"));

                unitOfWork.Accounts.Register(user);
                await unitOfWork.CompleteAsync();

            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception", ex);
            }
            return Ok();

        }
        /// <summary>
        /// Login a user.
        /// </summary>
        [HttpPost]
        [ValidationModel]
        public IActionResult Login([FromBody] LoginUserResource loginUserResource)
        {
            var user = mapper.Map<LoginUserResource, User>(loginUserResource);

            var userEmail = unitOfWork.Accounts.FindUserByEmail(user.Email);
            if (userEmail == null)
                return NotFound(new ApiResponse(404, "User not found"));
            
            var hashedPassword = userEmail.Password;

            if (!SecurePasswordHasherHelper.Verify(user.Password, hashedPassword)) 
                return Unauthorized(new ApiResponse(401, "Username or password don't match"));

           var token = unitOfWork.Accounts.Login(user, _auth);
           
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_Id = userEmail.Id,
                user_name = userEmail.Name,
                user_image = userEmail.ImageUrl
            });

        }
        /// <summary>
        /// Change password of user.
        /// </summary>
        [HttpPost]
        [Authorize]
        [ValidationModel]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordResource changePasswordResource)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = unitOfWork.Accounts.FindUserByEmail(userEmail);

            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));
          
            var hashedPassword = user.Password;
            if (!SecurePasswordHasherHelper.Verify(changePasswordResource.OldPassword, hashedPassword))
                return BadRequest(new ApiResponse(400, "Enter correct old password"));
            
            user.Password = SecurePasswordHasherHelper.Hash(changePasswordResource.NewPassword);
            await unitOfWork.CompleteAsync();

            return Ok(new ApiResponse(200,"Password changed successfully"));
        }

        /// <summary>
        /// Edit user profile.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(IFormFile file)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = unitOfWork.Accounts.FindUserByEmail(userEmail);

            //Validation Section
            if (user == null) 
                return BadRequest(new ApiResponse(400, "User does not exists"));

            if (file == null)
                return BadRequest(new ApiResponse(400, "File cannot be null"));

            if (file.Length == 0)
                return BadRequest(new ApiResponse(400, "Empty File"));

            if (file.Length > photoSettings.MaxBytes)
                return BadRequest(new ApiResponse(400, "Maximum File Size exceeded"));

            if (!photoSettings.IsSupported(file.FileName))
                return BadRequest(new ApiResponse(400, "Only Images allowed"));

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "profile");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            };
            Stream fileStream = file.OpenReadStream();
            Image newImage = photoSettings.GetReducedImage(100, 100, fileStream);

            var thumbnailFolderPath = Path.Combine(host.WebRootPath, "profile-thumbnails");
            if (!Directory.Exists(thumbnailFolderPath))
                Directory.CreateDirectory(thumbnailFolderPath);

            var thumbnailFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var thumbnailFilePath = Path.Combine(thumbnailFolderPath, thumbnailFileName);

            newImage.Save(thumbnailFilePath);
            user.ImageUrl = fileName;
            user.ThumbnailUrl = thumbnailFileName;

            await unitOfWork.CompleteAsync();
            return Ok(mapper.Map<User,UserResource>(user));
        }


    }
}