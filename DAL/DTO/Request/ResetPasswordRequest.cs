using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Request
{
  public class ResetPasswordRequest
    {
        public String Code { get; set; }
        public String NewPassword { get; set; }
        public String Email { get; set; }
    }
}
