using BLL.Service;
using DAL.DTO.Request;
using DAL.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace KAshop2Rep.Area.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationServicecs _authenticationServicecs;

        public AccountController(IAuthenticationServicecs authenticationServicecs)
        {
            _authenticationServicecs = authenticationServicecs;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(DAL.DTO.Request.LoginRequest loginRequest)
        {
            var responce = await _authenticationServicecs.LoginAsync(loginRequest);


            return Ok(responce);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Registor(RegestorRequest regestorRequest)
        {
            var responce = await _authenticationServicecs.RegistorAsync(regestorRequest);
            return Ok(responce);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var responce = await _authenticationServicecs.ConfirmEmailAsync(token,userId);
            return Ok(responce);
        }

        [HttpPost("ResetCode")]
        public async Task<IActionResult> ResetCode(ForgetPasswordRequest request
            )
        {
            var responce = await _authenticationServicecs.ReqestPasswordReset(request);

            if (responce is not null)
            {
                return Ok(responce);
              
            }
            return BadRequest(responce);
        }
        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword(DAL.DTO.Request.ResetPasswordRequest request)
        {
            var responce = await _authenticationServicecs.ResetPassword(request);

            if (responce is not null)
            {
                return Ok(responce);

            }
            return BadRequest(responce);
        }


        [HttpPatch("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenApiModel request)
        {
            var result = await _authenticationServicecs.RefreshToenAsync(request);
            if (result.Success=="false")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
