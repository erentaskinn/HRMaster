using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Models.DTOs.AppUserDTOs
{
    public class AppUserUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string YourMessage { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsCall { get; set; } 
        public bool? IsOk { get; set; } 

    }
}
