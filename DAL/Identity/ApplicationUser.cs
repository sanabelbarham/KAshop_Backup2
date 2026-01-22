using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
  public  class ApplicationUser:IdentityUser
    {

        public String FullName { get; set; }
        public String? City { get; set; }
        public String? Street { get; set; }
        public String? PaswordResetCode { get; set; }
        public DateTime? PaswordResetCodeExpiary { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
