using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Users
{
    public class User : BaseUser
    {
       
        public string AvatarUrl { get; set; }

        
        public string TenantId { get; set; }

       
        public string Password { get; set; }

        
        public string PasswordConfirm { get; set; }

       
        public DateTime DateCreated { get; set; }

        
        public DateTime DateModified { get; set; }
    }
}

