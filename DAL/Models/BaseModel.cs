using DAL.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
 public   class BaseModel
    {
        public int Id { get; set; }
        public string Language { get; set; } = "en";
        public string CreatedBy { get; set; }//this is ht euser id
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey("CreatedBy")]
        public ApplicationUser User { get; set; }

    }
}
