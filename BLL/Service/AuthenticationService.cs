using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Identity;
using DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class AuthenticationService : IAuthenticationServicecs
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration, ICategoryRepository categoryRepository, IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _categoryRepository = categoryRepository;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        public async Task<LoginResponce> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                return new LoginResponce
                {
                    Success = "false",
                    Message = "invalid email"

                };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {

                return new LoginResponce
                {
                    Success = "false",
                    Message = "the account is locked out  "
                };
            }


            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);



            if (result.IsLockedOut)
            {
                return new LoginResponce
                {
                    Success = "false",
                    Message = "the account is locked out due to multiple trys "
                };
            }
            else if (result.IsNotAllowed)
            {

                return new LoginResponce
                {
                    Success = "false",
                    Message = "plz confirm your email "
                };
            }

            else if (!result.Succeeded)
            {

                return new LoginResponce
                {
                    Success = "false",
                    Message = "invalid password try again "
                };
            }

            var accessToken = await _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);



            return new LoginResponce
            {
                Success = "true",
                Message = "Login successfully",
                AccessToken = await _tokenService.GenerateAccessToken(user),
                RefreshToken = refreshToken

            };

        }

        public async Task<RegestorResponce> RegistorAsync(RegestorRequest regestorRequest)
        {

            try
            {
                var user = regestorRequest.Adapt<ApplicationUser>();
                user.UserName = regestorRequest.Username.Trim();  // remove spaces

                var result = await _userManager.CreateAsync(user, regestorRequest.Password);
                if (!result.Succeeded)
                {
                    return new RegestorResponce
                    {
                        Message = "user register failed ",
                        Errors = result.Errors.Select(e => e.Description).ToList()

                    };
                }
                var role = await _userManager.AddToRoleAsync(user, "User");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var emailUrl = $"https://localhost:7122/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
                await _emailSender.SendEmailAsync(user.Email, "Welcome ", $"<h1>sanabel are you registering to this website?... {user.UserName}</h1> " +
                    $"<a href ={emailUrl}>confirm email</a>");

                return new RegestorResponce
                {
                    Message = "success"
                };
            }
            catch (Exception ex)
            {
                return new RegestorResponce
                {
                    Success = "false",
                    Message = "an unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }

        }



        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;
            }
            //this function is ready from the identity changes the EmailConframe
            //from false to ture when user clicks the link

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }


        public async Task<ForgetPasswordResponce> ReqestPasswordReset(ForgetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgetPasswordResponce
                {
                    Success = "false",
                    Message = "no user with this email "
                };
            }
            var random = new Random();
            var code = random.Next(200, 10000).ToString();
            user.PaswordResetCode = code;
            user.PaswordResetCodeExpiary = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailSender.SendEmailAsync(user.Email, "reset password", $"<h1> the code is {code}</h1>");
            return new ForgetPasswordResponce
            {
                Message = "code sent succefully",
                Success = "true"
            };


        }


        public async Task<ResetPasswordResponce> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponce
                {
                    Success = "false",
                    Message = "no user with this email "
                };
            }

            if (user.PaswordResetCode != request.Code)
            {
                return new ResetPasswordResponce
                {
                    Success = "false",
                    Message = "the code is invalid"
                };
            }
            if (user.PaswordResetCodeExpiary < DateTime.UtcNow)
            {
                return new ResetPasswordResponce
                {
                    Success = "false",
                    Message = "the code is Expired"
                };

            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponce
                {
                    Success = "false",
                    Message = "password reset failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _emailSender.SendEmailAsync(request.Email, "password reset", "<h1>password reset is done</h1>");
            return new ResetPasswordResponce
            {
                Message = "Password Reset succefully",
                Success = "true"
            };


        }

        public async Task<LoginResponce> RefreshToenAsync(TokenApiModel tokenApiModel)
        {


            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var priniple = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = priniple.Identity.Name;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new LoginResponce()
                {
                    Success = "false",
                    Message = "invalid client request"
                };
            }
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken =await  _tokenService.GenerateRefreshToken();
            await _userManager.UpdateAsync(user);
            return new LoginResponce
            {
                Success = "true",
                Message = "Token Refreshed",
                AccessToken = newAccessToken,
                RefreshToken =  newRefreshToken,
            };

        }
    }
}
