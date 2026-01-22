using DAL.DTO.Request;
using DAL.DTO.Response;
using DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
   public interface IAuthenticationServicecs
    {
        Task<RegestorResponce> RegistorAsync(RegestorRequest regestorRequest);
        Task<LoginResponce> LoginAsync(LoginRequest loginRequest);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<ForgetPasswordResponce> ReqestPasswordReset(ForgetPasswordRequest request);
        Task<ResetPasswordResponce> ResetPassword(ResetPasswordRequest request);
        Task<LoginResponce> RefreshToenAsync(TokenApiModel tokenApiModel);

    }
}
